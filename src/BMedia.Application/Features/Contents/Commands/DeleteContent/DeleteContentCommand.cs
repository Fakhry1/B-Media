using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Commands.DeleteContent;

public record DeleteContentCommand(Guid ContentId) : IRequest<Result<bool>>;

public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;

    public DeleteContentCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents.FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);
        if (content is null) return Result<bool>.NotFound("Content not found");

        if (content.Status == ContentStatus.Published)
            return Result<bool>.Failure("Cannot delete published content. Archive it first.");

        content.IsDeleted = true;
        content.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
