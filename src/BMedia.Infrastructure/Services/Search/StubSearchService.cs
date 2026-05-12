using BMedia.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BMedia.Infrastructure.Services.Search;

/// <summary>
/// Stub search service. Replace with Elasticsearch/OpenSearch implementation.
/// The interface contract is stable — swapping providers requires no changes to Application layer.
/// </summary>
public class StubSearchService : ISearchService
{
    private readonly ILogger<StubSearchService> _logger;

    public StubSearchService(ILogger<StubSearchService> logger) => _logger = logger;

    public Task IndexContentAsync(Guid contentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Search: indexing content {ContentId}", contentId);
        return Task.CompletedTask;
    }

    public Task RemoveContentIndexAsync(Guid contentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Search: removing content index {ContentId}", contentId);
        return Task.CompletedTask;
    }

    public Task<SearchResult> SearchContentsAsync(SearchQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Search: stub implementation returning empty results");
        return Task.FromResult(new SearchResult([], 0, query.Page, query.PageSize));
    }
}
