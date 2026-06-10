using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Subcategories.Queries;

public record SubcategoryDetailDto(
    Guid   Id,
    Guid   CategoryId,
    string CategoryName,
    string Name,
    string Slug,
    string? Description,
    int    SortOrder,
    bool   IsActive,
    DateTime CreatedAt
);

public record GetSubcategoriesQuery(Guid? CategoryId = null, bool ActiveOnly = true)
    : IRequest<Result<IEnumerable<SubcategoryDetailDto>>>;

public class GetSubcategoriesQueryHandler
    : IRequestHandler<GetSubcategoriesQuery, Result<IEnumerable<SubcategoryDetailDto>>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public GetSubcategoriesQueryHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<IEnumerable<SubcategoryDetailDto>>> Handle(
        GetSubcategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Subcategories
            .AsNoTracking()
            .Include(s => s.Category)
            .Where(s => !s.IsDeleted);

        if (request.CategoryId.HasValue)
            query = query.Where(s => s.CategoryId == request.CategoryId.Value);

        if (request.ActiveOnly)
            query = query.Where(s => s.IsActive);

        var result = await query
            .OrderBy(s => s.SortOrder).ThenBy(s => s.Name)
            .Select(s => new SubcategoryDetailDto(
                s.Id, s.CategoryId, s.Category.Name,
                s.Name, s.Slug, s.Description,
                s.SortOrder, s.IsActive, s.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<SubcategoryDetailDto>>.Success(result);
    }
}
