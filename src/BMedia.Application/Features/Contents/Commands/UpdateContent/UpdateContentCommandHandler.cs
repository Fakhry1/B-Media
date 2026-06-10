using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Commands.UpdateContent;

public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;

    public UpdateContentCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents
            .Include(c => c.ContentTags)
            .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

        if (content is null) return Result<bool>.NotFound("Content not found");

        if (content.Status is ContentStatus.Published or ContentStatus.Archived)
            return Result<bool>.Failure("Cannot edit published or archived content");

        content.Title = request.Title;
        content.Summary = request.Summary;
        content.Language = request.Language;
        content.CategoryId = request.CategoryId;
        content.SubcategoryId = request.SubcategoryId;
        content.IsFeatured = request.IsFeatured;
        content.AllowComments = request.AllowComments;

        if (request.TagIds is not null)
        {
            _db.ContentTags.RemoveRange(content.ContentTags);
            var validTagIds = await _db.Tags.Where(t => request.TagIds.Contains(t.Id)).Select(t => t.Id).ToListAsync(cancellationToken);
            content.ContentTags = validTagIds.Select(tid => new ContentTag { ContentId = content.Id, TagId = tid }).ToList();
        }

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
