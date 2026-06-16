using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BMedia.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BMedia.Infrastructure.Services.Auth;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpiryMinutes { get; set; } = 15;
    public int RefreshTokenExpiryDays { get; set; } = 30;
}

public interface IJwtService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    /// <summary>Validates an active (non-expired) token. Returns null if invalid or expired.</summary>
    ClaimsPrincipal? ValidateToken(string token);
    /// <summary>Validates signature and claims of an expired token — used only during refresh-token rotation.</summary>
    ClaimsPrincipal? ValidateExpiredToken(string token);
}

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;
    private readonly IDateTimeService _dateTimeService;

    public JwtService(IOptions<JwtSettings> settings, IDateTimeService dateTimeService)
    {
        _settings = settings.Value;
        _dateTimeService = dateTimeService;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey))
        {
            KeyId = "bmedia-key-1"
        };
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: _dateTimeService.UtcNow,
            expires: _dateTimeService.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? ValidateToken(string token)
        => ValidateTokenCore(token, validateLifetime: true);

    /// <summary>
    /// Used ONLY for refresh-token rotation: the access token is intentionally expired,
    /// but we still need to verify its signature and extract claims from it.
    /// </summary>
    public ClaimsPrincipal? ValidateExpiredToken(string token)
        => ValidateTokenCore(token, validateLifetime: false);

    private ClaimsPrincipal? ValidateTokenCore(string token, bool validateLifetime)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.SecretKey);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
