using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Subcategories.Commands;

public record UpdateSubcategoryCommand(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder,
    bool IsActive
) : IRequest<Result<bool>>;

public class UpdateSubcategoryCommandHandler : IRequestHandler<UpdateSubcategoryCommand, Result<bool>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public UpdateSubcategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategory = await _db.Subcategories
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (subcategory is null)
            return Result<bool>.Failure("Subcategory not found", 404);

        var newSlug = request.Name.ToLowerInvariant().Replace(" ", "-");

        if (newSlug != subcategory.Slug)
        {
            var slugExists = await _db.Subcategories
                .AnyAsync(s => s.Slug == newSlug && s.Id != request.Id && !s.IsDeleted, cancellationToken);

            if (slugExists)
                return Result<bool>.Failure($"A subcategory with slug '{newSlug}' already exists", 409);
        }

        subcategory.Name        = request.Name;
        subcategory.Slug        = newSlug;
        subcategory.Description = request.Description;
        subcategory.SortOrder   = request.SortOrder;
        subcategory.IsActive    = request.IsActive;

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
