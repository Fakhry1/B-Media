using Asp.Versioning;
using BMedia.Application.Features.Categories.Commands;
using BMedia.Application.Features.Categories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

/// <summary>Category and subcategory taxonomy management.</summary>
[ApiVersion("1.0")]
public class CategoriesController : BaseApiController
{
    /// <summary>Get all active categories with subcategories.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] bool includeSubcategories = true, CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetCategoriesQuery(includeSubcategories), cancellationToken));

    /// <summary>Create a new category.</summary>
    [HttpPost]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));

    /// <summary>Update an existing category.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(
            new UpdateCategoryCommand(id, request.Name, request.Description, request.IconUrl, request.SortOrder, request.IsActive),
            cancellationToken));

    /// <summary>Delete (soft-delete) a category.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new DeleteCategoryCommand(id), cancellationToken));
}

public record UpdateCategoryRequest(string Name, string? Description, string? IconUrl, int SortOrder, bool IsActive = true);
