using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public DeleteCategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _db.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Result<bool>.Failure("Category not found", 404);

        category.IsDeleted = true;
        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
