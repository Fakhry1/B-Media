using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(Guid Id, string Name, string? Description, string? IconUrl, int SortOrder, bool IsActive) : IRequest<Result<bool>>;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public UpdateCategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _db.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Result<bool>.Failure("Category not found", 404);

        category.Name        = request.Name;
        category.Slug        = request.Name.ToLowerInvariant().Replace(" ", "-");
        category.Description = request.Description;
        category.IconUrl     = request.IconUrl;
        category.SortOrder   = request.SortOrder;
        category.IsActive    = request.IsActive;

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
