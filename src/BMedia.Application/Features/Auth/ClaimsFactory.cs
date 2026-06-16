using System.Security.Claims;

namespace BMedia.Application.Features.Auth;

/// <summary>Single source of truth for JWT claim layout used across login and token refresh.</summary>
internal static class ClaimsFactory
{
    internal static IEnumerable<Claim> BuildClaims(
        Guid userId, string email, string username,
        string firstName, string lastName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, $"{firstName} {lastName}"),
            new("preferred_username", username),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        return claims;
    }
}
