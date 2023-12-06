using CacheSample.Shared.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace CacheSample.Infra.Caching;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;

    public CacheService(IConnectionMultiplexer redis)
    {
        _cacheDb = redis.GetDatabase();
    }

    public async Task<IEnumerable<T>> GetAllDataAsync<T>(string hashKey) where T : class
    {
        var cachedValue = await _cacheDb.HashGetAllAsync(hashKey);

        if (cachedValue is null)
            return Enumerable.Empty<T>();

        var convertedHashData = Array.ConvertAll(cachedValue, val => JsonSerializer.Deserialize<T>(val.Value)).ToList();

        return convertedHashData;
    }

    public async Task<T?> GetDataByIdAsync<T>(string hashey, int dataId) where T : class
    {
        string? stringCachedValue = await _cacheDb.HashGetAsync(hashey, dataId);

        T? cachedValue = null;

        if (stringCachedValue is not null)
            cachedValue = JsonSerializer.Deserialize<T>(stringCachedValue);

        return cachedValue;
    }

    public async Task SetDataAsync<T>(string hashey, int dataId, T value) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        await _cacheDb.HashSetAsync(hashey, new HashEntry[]
        {
            new HashEntry(dataId, cacheValue)
        });
    }


    public async Task RemoveDataAsync(string key)
    {
        var exists = _cacheDb.KeyExists(key);

        if (exists)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
}
