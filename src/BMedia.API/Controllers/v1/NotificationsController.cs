using Asp.Versioning;
using BMedia.Application.Features.Notifications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>User notification management.</summary>
[ApiVersion("1.0")]
[Authorize]
public class NotificationsController : BaseApiController
{
    /// <summary>Get notifications for the authenticated user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool unreadOnly = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetNotificationsQuery(unreadOnly, page, pageSize), cancellationToken));
}
