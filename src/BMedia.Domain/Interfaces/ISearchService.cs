namespace BMedia.Domain.Interfaces;

public interface ISearchService
{
    Task IndexContentAsync(Guid contentId, CancellationToken cancellationToken = default);
    Task RemoveContentIndexAsync(Guid contentId, CancellationToken cancellationToken = default);
    Task<SearchResult> SearchContentsAsync(SearchQuery query, CancellationToken cancellationToken = default);
}

public record SearchQuery(
    string? Term,
    IEnumerable<string>? Tags = null,
    IEnumerable<Guid>? CategoryIds = null,
    string? Language = null,
    int Page = 1,
    int PageSize = 20
);

public record SearchResult(
    IEnumerable<SearchHit> Hits,
    long TotalCount,
    int Page,
    int PageSize
);

public record SearchHit(
    Guid ContentId,
    string Title,
    string? Summary,
    double Score
);
