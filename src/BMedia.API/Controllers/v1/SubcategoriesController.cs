using Asp.Versioning;
using BMedia.Application.Features.Subcategories.Commands;
using BMedia.Application.Features.Subcategories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>Subcategory management within a parent category.</summary>
[ApiVersion("1.0")]
public class SubcategoriesController : BaseApiController
{
    /// <summary>Get subcategories — optionally filtered by category.</summary>
    /// <param name="categoryId">Filter by parent category ID (optional).</param>
    /// <param name="activeOnly">Return only active subcategories (default true).</param>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? categoryId = null,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetSubcategoriesQuery(categoryId, activeOnly), cancellationToken));

    /// <summary>Get a single subcategory by ID.</summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new GetSubcategoryByIdQuery(id), cancellationToken));

    /// <summary>Create a new subcategory inside a category.</summary>
    [HttpPost]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Create(
        [FromBody] CreateSubcategoryCommand command,
        CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));

    /// <summary>Update an existing subcategory.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSubcategoryRequest request,
        CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(
            new UpdateSubcategoryCommand(id, request.Name, request.Description, request.SortOrder, request.IsActive),
            cancellationToken));

    /// <summary>Soft-delete a subcategory.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new DeleteSubcategoryCommand(id), cancellationToken));
}

/// <summary>Request body for updating a subcategory.</summary>
public record UpdateSubcategoryRequest(
    string Name,
    string? Description,
    int SortOrder,
    bool IsActive
);
