using CacheSample.Shared.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CacheSample.Infra.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
            return null;

        T? value = JsonSerializer.Deserialize<T>(cachedValue);

        return value;
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync(key, cachedValue, cancellationToken);

        return cachedValue;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
    }

    public async Task RemoveAsync<T>(string key, CancellationToken cancellationToken = default) =>
        await _distributedCache.RemoveAsync(key, cancellationToken);
}
