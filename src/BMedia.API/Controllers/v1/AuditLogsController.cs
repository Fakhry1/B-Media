using Asp.Versioning;
using BMedia.Application.Features.AuditLogs.Queries;
using BMedia.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>Audit log access — Administrator only.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = "Administrator")]
public class AuditLogsController : BaseApiController
{
    /// <summary>Query audit logs with filters.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] string? entityType,
        [FromQuery] AuditAction? action,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetAuditLogsQuery(userId, entityType, action, from, to, page, pageSize), cancellationToken));
}
