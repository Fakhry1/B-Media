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
    public async Task<IActionResult> GetAll([FromQuery] bool includeSubcategories = true, CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetCategoriesQuery(includeSubcategories), cancellationToken));

    /// <summary>Create a new category.</summary>
    [HttpPost]
    [Authorize(Policy = "ManageCategories")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(command, cancellationToken));
}
