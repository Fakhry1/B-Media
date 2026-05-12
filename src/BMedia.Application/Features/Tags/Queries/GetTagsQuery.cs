using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Tags.Queries;

public record GetTagsQuery(string? Search = null, int Page = 1, int PageSize = 50) : IRequest<Result<PaginatedResult<TagDto>>>;

public record TagDto(Guid Id, string Name, string Slug, string? Description, bool IsAiGenerated, int UsageCount);

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Result<PaginatedResult<TagDto>>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public GetTagsQueryHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<PaginatedResult<TagDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Tags.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(t => t.Name.Contains(request.Search));

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(t => t.Name)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(t => new TagDto(t.Id, t.Name, t.Slug, t.Description, t.IsAiGenerated, t.ContentTags.Count))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<TagDto>>.Success(new PaginatedResult<TagDto>(items, total, request.Page, request.PageSize));
    }
}
