using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Subcategories.Commands;

public record DeleteSubcategoryCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteSubcategoryCommandHandler : IRequestHandler<DeleteSubcategoryCommand, Result<bool>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public DeleteSubcategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategory = await _db.Subcategories
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (subcategory is null)
            return Result<bool>.Failure("Subcategory not found", 404);

        subcategory.IsDeleted  = true;
        subcategory.DeletedAt  = DateTime.UtcNow;
        subcategory.IsActive   = false;

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, 204);
    }
}
