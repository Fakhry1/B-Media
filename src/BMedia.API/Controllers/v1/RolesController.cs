using Asp.Versioning;
using BMedia.Application.Features.Roles.Commands.CreateRole;
using BMedia.Application.Features.Roles.Commands.DeleteRole;
using BMedia.Application.Features.Roles.Commands.UpdateRole;
using BMedia.Application.Features.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BMedia.API.Controllers.v1;

/// <summary>Role management — Administrator role required.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = "Administrator")]
[EnableRateLimiting("api")]
public class RolesController : BaseApiController
{
    /// <summary>List all roles with optional search filter.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetRolesQuery(search), cancellationToken));

    /// <summary>Create a new role.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoleCreatedResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));

    /// <summary>Update an existing role's name and description.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(id, request.Name, request.Description);
        return ToActionResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>Soft-delete a role. System roles cannot be deleted.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new DeleteRoleCommand(id), cancellationToken));
}

public record UpdateRoleRequest(string Name, string? Description);
