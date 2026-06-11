using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>Development-only diagnostics — not available in Production.</summary>
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class DebugController : BaseApiController
{
    private readonly IWebHostEnvironment _env;

    public DebugController(IWebHostEnvironment env) => _env = env;

    /// <summary>Returns the authenticated user's claims. Development only.</summary>
    [HttpGet("claims")]
    [Authorize]
    public IActionResult Claims()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Name = User.Identity?.Name,
            Claims = claims,
            Roles = User.Claims.Where(c =>
                c.Type == System.Security.Claims.ClaimTypes.Role ||
                c.Type == "role").Select(c => c.Value),
            Permissions = User.Claims.Where(c => c.Type == "permission").Select(c => c.Value)
        });
    }
}
