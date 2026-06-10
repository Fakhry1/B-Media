using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Subcategories.Commands;

public record CreateSubcategoryCommand(
    Guid CategoryId,
    string Name,
    string? Description,
    int SortOrder = 0
) : IRequest<Result<Guid>>;

public class CreateSubcategoryCommandHandler : IRequestHandler<CreateSubcategoryCommand, Result<Guid>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public CreateSubcategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<Guid>> Handle(CreateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _db.Categories
            .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted, cancellationToken);

        if (!categoryExists)
            return Result<Guid>.Failure("Category not found", 404);

        var slug = request.Name.ToLowerInvariant().Replace(" ", "-");

        var slugExists = await _db.Subcategories
            .AnyAsync(s => s.Slug == slug && !s.IsDeleted, cancellationToken);

        if (slugExists)
            return Result<Guid>.Failure($"A subcategory with slug '{slug}' already exists", 409);

        var subcategory = new Domain.Entities.Subcategory
        {
            CategoryId  = request.CategoryId,
            Name        = request.Name,
            Slug        = slug,
            Description = request.Description,
            SortOrder   = request.SortOrder
        };

        _db.Subcategories.Add(subcategory);
        await _db.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Created(subcategory.Id);
    }
}
