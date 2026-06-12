using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Commands.UpdateContentStatus;

public record UpdateContentStatusCommand(Guid ContentId, ContentStatus Status, string? Comment)
    : IRequest<Result<bool>>;

public class UpdateContentStatusCommandHandler
    : IRequestHandler<UpdateContentStatusCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;

    public UpdateContentStatusCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(
        UpdateContentStatusCommand request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents
            .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

        if (content is null)
            return Result<bool>.NotFound("Content not found");

        content.Status = request.Status;

        if (request.Status == ContentStatus.Published && content.PublishedAt is null)
            content.PublishedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
