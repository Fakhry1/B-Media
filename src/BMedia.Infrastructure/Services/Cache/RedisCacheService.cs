using System.Text.Json;
using BMedia.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BMedia.Infrastructure.Services.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetStringAsync(key, cancellationToken);
        return data is null ? default : JsonSerializer.Deserialize<T>(data, JsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions();
        if (expiry.HasValue) options.AbsoluteExpirationRelativeToNow = expiry;

        var data = JsonSerializer.Serialize(value, JsonOptions);
        await _cache.SetStringAsync(key, data, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await _cache.RemoveAsync(key, cancellationToken);

    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // Requires IConnectionMultiplexer + SCAN command — not available via IDistributedCache.
        // Inject IConnectionMultiplexer directly when this is needed in production.
        throw new NotSupportedException(
            "Pattern-based cache invalidation requires a direct Redis connection (IConnectionMultiplexer). " +
            "Implement via SCAN + DEL pipeline when needed.");
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached is not null) return cached;

        var value = await factory();
        await SetAsync(key, value, expiry, cancellationToken);
        return value;
    }
}
