using Asp.Versioning;
using BMedia.Application.Features.Contents.Commands.CreateContent;
using BMedia.Application.Features.Contents.Commands.DeleteContent;
using BMedia.Application.Features.Contents.Commands.TransitionWorkflow;
using BMedia.Application.Features.Contents.Commands.UpdateContent;
using BMedia.Application.Features.Contents.Queries.GetContentById;
using BMedia.Application.Features.Contents.Queries.GetContents;
using BMedia.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BMedia.API.Controllers.v1;

/// <summary>Content lifecycle management.</summary>
[ApiVersion("1.0")]
[Authorize]
[EnableRateLimiting("api")]
public class ContentsController : BaseApiController
{
    /// <summary>List contents with filtering, sorting, and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] ContentStatus? status = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] string? language = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetContentsQuery(page, pageSize, search, status, categoryId, language, isFeatured, sortBy, sortDescending), cancellationToken));

    /// <summary>Get a content item by ID.</summary>
    [HttpGet("{id:guid}")]
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
        var command = new UpdateContentCommand(id, request.Title, request.Summary, request.Body, request.Language,
            request.CategoryId, request.SubcategoryId, request.SeoTitle, request.SeoDescription, request.SeoKeywords,
            request.IsFeatured, request.AllowComments, request.TagIds);
        return ToActionResult(await Sender.Send(command, cancellationToken));
    }

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
    string Title, string? Summary, string? Body, string Language,
    Guid? CategoryId, Guid? SubcategoryId, string? SeoTitle, string? SeoDescription, string? SeoKeywords,
    bool IsFeatured, bool AllowComments, IEnumerable<Guid>? TagIds);

public record TransitionWorkflowRequest(Guid TransitionId, string? Comment);
