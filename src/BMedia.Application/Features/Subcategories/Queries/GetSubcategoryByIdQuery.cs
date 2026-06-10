using BMedia.Application.Common.Models;
using BMedia.Application.Features.Subcategories.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Subcategories.Queries;

public record GetSubcategoryByIdQuery(Guid Id) : IRequest<Result<SubcategoryDetailDto>>;

public class GetSubcategoryByIdQueryHandler
    : IRequestHandler<GetSubcategoryByIdQuery, Result<SubcategoryDetailDto>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public GetSubcategoryByIdQueryHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<SubcategoryDetailDto>> Handle(
        GetSubcategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var s = await _db.Subcategories
            .AsNoTracking()
            .Include(s => s.Category)
            .Where(s => s.Id == request.Id && !s.IsDeleted)
            .Select(s => new SubcategoryDetailDto(
                s.Id, s.CategoryId, s.Category.Name,
                s.Name, s.Slug, s.Description,
                s.SortOrder, s.IsActive, s.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (s is null)
            return Result<SubcategoryDetailDto>.Failure("Subcategory not found", 404);

        return Result<SubcategoryDetailDto>.Success(s);
    }
}
