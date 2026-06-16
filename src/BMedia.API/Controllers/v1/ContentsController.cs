using Asp.Versioning;
using BMedia.Application.Features.Contents.Commands.CreateContent;
using BMedia.Application.Features.Contents.Commands.DeleteContent;
using BMedia.Application.Features.Contents.Commands.TransitionWorkflow;
using BMedia.Application.Features.Contents.Commands.UpdateContent;
using BMedia.Application.Features.Contents.Commands.UpdateContentStatus;
using BMedia.Application.Features.Contents.Queries.GetAvailableTransitions;
using BMedia.Application.Features.Contents.Queries.GetContentById;
using BMedia.Application.Features.Contents.Queries.GetContents;
using BMedia.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.ComponentModel.DataAnnotations;

namespace BMedia.API.Controllers.v1;

/// <summary>Content lifecycle management.</summary>
[ApiVersion("1.0")]
[Authorize]
[EnableRateLimiting("api")]
public class ContentsController : BaseApiController
{
    /// <summary>List contents with filtering, sorting, and pagination.</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery][Range(1, int.MaxValue)] int page = 1,
        [FromQuery][Range(1, 100)] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] ContentStatus? status = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] Guid? subcategoryId = null,
        [FromQuery] string? language = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true,
        [FromQuery] MediaType? mediaType = null,
        CancellationToken cancellationToken = default)
    {
        // Anonymous callers may only see published content.
        if (!User.Identity?.IsAuthenticated == true)
            status ??= ContentStatus.Published;

        return ToActionResult(await Sender.Send(
            new GetContentsQuery(page, pageSize, search, status, categoryId, subcategoryId,
                language, isFeatured, sortBy, sortDescending, mediaType),
            cancellationToken));
    }

    /// <summary>Get a content item by ID.</summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new GetContentByIdQuery(id), cancellationToken));

    /// <summary>Create a new content item.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateContentResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContentCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));

    /// <summary>Update an existing content item.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContentRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateContentCommand(id, request.Title, request.Summary, request.Language,
            request.CategoryId, request.SubcategoryId,
            request.IsFeatured, request.AllowComments, request.TagIds);
        return ToActionResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>Directly update content status (bypasses workflow — for admins).</summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(
            new UpdateContentStatusCommand(id, request.Status, request.Comment), cancellationToken));

    /// <summary>Get available workflow transitions for a content item.</summary>
    [HttpGet("{id:guid}/workflow/transitions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransitions(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new GetAvailableTransitionsQuery(id), cancellationToken));

    /// <summary>Transition content through a workflow step.</summary>
    [HttpPost("{id:guid}/workflow/transition")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TransitionWorkflow(Guid id, [FromBody] TransitionWorkflowRequest request, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new TransitionWorkflowCommand(id, request.TransitionId, request.Comment), cancellationToken));
}

public record UpdateContentRequest(
    string Title, string? Summary, string Language,
    Guid? CategoryId, Guid? SubcategoryId,
    bool IsFeatured, bool AllowComments, IEnumerable<Guid>? TagIds);

public record TransitionWorkflowRequest(Guid TransitionId, string? Comment);
public record UpdateStatusRequest(ContentStatus Status, string? Comment);
