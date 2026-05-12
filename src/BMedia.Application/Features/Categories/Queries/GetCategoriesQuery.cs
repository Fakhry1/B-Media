using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Categories.Queries;

public record GetCategoriesQuery(bool IncludeSubcategories = true) : IRequest<Result<IEnumerable<CategoryDto>>>;

public record CategoryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? IconUrl,
    int SortOrder,
    IEnumerable<SubcategoryDto> Subcategories
);

public record SubcategoryDto(Guid Id, string Name, string Slug, int SortOrder);

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public GetCategoriesQueryHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Categories.AsNoTracking().Where(c => c.IsActive);

        if (request.IncludeSubcategories)
            query = query.Include(c => c.Subcategories.Where(s => s.IsActive && !s.IsDeleted));

        var categories = await query.OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
            .Select(c => new CategoryDto(
                c.Id, c.Name, c.Slug, c.Description, c.IconUrl, c.SortOrder,
                c.Subcategories.OrderBy(s => s.SortOrder).Select(s => new SubcategoryDto(s.Id, s.Name, s.Slug, s.SortOrder))))
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<CategoryDto>>.Success(categories);
    }
}
