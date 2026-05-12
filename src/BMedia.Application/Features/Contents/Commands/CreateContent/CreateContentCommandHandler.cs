using BMedia.Application.Common.Interfaces;
using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Commands.CreateContent;

public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, Result<CreateContentResult>>
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateContentCommandHandler(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateContentResult>> Handle(CreateContentCommand request, CancellationToken cancellationToken)
    {
        var slug = await GenerateUniqueSlugAsync(request.Title, cancellationToken);

        var content = new Content
        {
            Title = request.Title,
            Slug = slug,
            Summary = request.Summary,
            Body = request.Body,
            Language = request.Language,
            CategoryId = request.CategoryId,
            SubcategoryId = request.SubcategoryId,
            SeoTitle = request.SeoTitle,
            SeoDescription = request.SeoDescription,
            SeoKeywords = request.SeoKeywords,
            ScheduledPublishAt = request.ScheduledPublishAt,
            Status = ContentStatus.Draft
        };

        if (request.TagIds?.Any() == true)
        {
            var existingTagIds = await _db.Tags
                .Where(t => request.TagIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

            content.ContentTags = existingTagIds
                .Select(tagId => new ContentTag { ContentId = content.Id, TagId = tagId })
                .ToList();
        }

        _db.Contents.Add(content);
        await _db.SaveChangesAsync(cancellationToken);

        return Result<CreateContentResult>.Created(new CreateContentResult(content.Id, content.Slug));
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, CancellationToken cancellationToken)
    {
        var baseSlug = title.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("--", "-")
            .Trim('-');

        // Strip non-alphanumeric/dash chars
        baseSlug = new string(baseSlug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

        var slug = baseSlug;
        var counter = 1;

        while (await _db.Contents.AnyAsync(c => c.Slug == slug, cancellationToken))
        {
            slug = $"{baseSlug}-{counter++}";
        }

        return slug;
    }
}
