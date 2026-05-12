using Asp.Versioning;
using BMedia.Application.Features.Tags.Commands;
using BMedia.Application.Features.Tags.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>Tag management for content taxonomy.</summary>
[ApiVersion("1.0")]
[Authorize]
public class TagsController : BaseApiController
{
    /// <summary>Search and list tags.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetTagsQuery(search, page, pageSize), cancellationToken));

    /// <summary>Create a new tag.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));
}
