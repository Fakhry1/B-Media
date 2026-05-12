using System.Security.Claims;
using BMedia.Application.Common.Models;
using BMedia.Application.Features.Auth.Commands.Login;
using BMedia.Infrastructure.Persistence;
using BMedia.Infrastructure.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BMedia.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResult>>
{
    private readonly ApplicationDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(ApplicationDbContext db, IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
    {
        _db = db;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtService.ValidateToken(request.AccessToken);
        if (principal is null) return Result<LoginResult>.Unauthorized("Invalid access token");

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId)) return Result<LoginResult>.Unauthorized();

        var storedToken = await _db.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId, cancellationToken);

        if (storedToken is null || !storedToken.IsActive)
            return Result<LoginResult>.Unauthorized("Invalid or expired refresh token");

        var user = storedToken.User;

        // Rotate refresh token
        storedToken.IsRevoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.RevokedByIp = request.IpAddress;
        storedToken.RevokedReason = "Replaced by new token";

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.Name).Distinct().ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new("preferred_username", user.Username)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var newAccessToken = _jwtService.GenerateAccessToken(claims);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        var newTokenEntity = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            CreatedByIp = request.IpAddress
        };

        storedToken.ReplacedByToken = newRefreshToken;
        _db.RefreshTokens.Add(newTokenEntity);
        await _db.SaveChangesAsync(cancellationToken);

        return Result<LoginResult>.Success(new LoginResult(
            newAccessToken,
            newRefreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            new UserInfo(user.Id, user.Email, user.Username, user.FirstName, user.LastName, roles, permissions)
        ));
    }
}
