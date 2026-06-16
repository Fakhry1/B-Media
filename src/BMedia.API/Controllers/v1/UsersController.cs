using Asp.Versioning;
using BMedia.Application.Features.Users.Commands.AssignRoles;
using BMedia.Application.Features.Users.Commands.CreateUser;
using BMedia.Application.Features.Users.Commands.DeleteUser;
using BMedia.Application.Features.Users.Commands.ResetPassword;
using BMedia.Application.Features.Users.Commands.UpdateUser;
using BMedia.Application.Features.Users.Queries.GetUserById;
using BMedia.Application.Features.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.ComponentModel.DataAnnotations;

namespace BMedia.API.Controllers.v1;

/// <summary>User management — Administrator role required.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = "Administrator")]
[EnableRateLimiting("api")]
public class UsersController : BaseApiController
{
    /// <summary>List users with optional filtering and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery][Range(1, int.MaxValue)] int page = 1,
        [FromQuery][Range(1, 100)] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetUsersQuery(page, pageSize, search, isActive), cancellationToken));

    /// <summary>Get a user by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new GetUserByIdQuery(id), cancellationToken));

    /// <summary>Create a new user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserCreatedResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));

    /// <summary>Update an existing user's profile fields.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.PhoneNumber, request.PreferredLanguage, request.IsActive);
        return ToActionResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>Soft-delete a user.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new DeleteUserCommand(id), cancellationToken));

    /// <summary>Replace the full set of roles assigned to a user.</summary>
    [HttpPut("{id:guid}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRoles(Guid id, [FromBody] AssignRolesRequest request, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new AssignUserRolesCommand(id, request.RoleIds), cancellationToken));

    /// <summary>Reset a user's password (admin action).</summary>
    [HttpPost("{id:guid}/reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new ResetUserPasswordCommand(id, request.NewPassword), cancellationToken));
}

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string PreferredLanguage,
    bool IsActive);

public record AssignRolesRequest(IEnumerable<Guid> RoleIds);
public record ResetPasswordRequest(string NewPassword);
