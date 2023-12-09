using CacheSample.Shared.Interfaces;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;
using System.Text.Json;

namespace CacheSample.Infra.Caching;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    private readonly ISearchCommands _ftCommands;
    private readonly IJsonCommands _jsonCommands;

    private string _indexName = string.Empty;
    private string _dataPrefix = string.Empty;

    public CacheService(IConnectionMultiplexer redis)
    {
        _cacheDb = redis.GetDatabase();

        _ftCommands = _cacheDb.FT();

        _jsonCommands = _cacheDb.JSON();
    }

    public void CreateIndex(Schema schema, string indexName, string dataPrefix)
    {
        _indexName = indexName;

        _dataPrefix = dataPrefix;

        var indexInfo = _ftCommands.Info(indexName);

        if (indexInfo is not null)
            return;

        _ftCommands.Create(
            indexName,
            new FTCreateParams()
                .On(IndexDataType.JSON)
                .Prefix(dataPrefix),
            schema
        );
    }



    public IEnumerable<T> GetAllData<T>() where T : class
    {
        var cachedValue = _ftCommands.Search(
            _indexName,
            new Query())
            .Documents.Select(x => x["json"]).ToArray();

        if (cachedValue is null)
            return Enumerable.Empty<T>();

        return Array.ConvertAll(cachedValue, val => JsonSerializer.Deserialize<T>(val)).ToList();
    }

    public IEnumerable<T> GetAllPaginatedData<T>(string orderBy, int pageNumber, int pageSize) where T : class
    {
        var cachedValue = _ftCommands.Search(
                _indexName,
                new Query()
                    .SetSortBy(orderBy)
                    .Limit((pageNumber - 1) * pageSize, pageSize)
            )
            .Documents.Select(x => x["json"])
            .ToArray();

        if (cachedValue is null)
            return Enumerable.Empty<T>();

        return Array.ConvertAll(cachedValue, val => JsonSerializer.Deserialize<T>(val)).ToList();
    }

    public T? GetDataById<T>(int dataId) where T : class
    {
        var stringCachedValue = _ftCommands.Search(
            _indexName,
            new Query($"@id:[{dataId} {dataId}]"))
            .Documents.Select(x => x["json"])
            .FirstOrDefault();

        if (stringCachedValue.HasValue)
            return JsonSerializer.Deserialize<T>(stringCachedValue.ToString());

        return null;
    }

    public void SetData<T>(int dataId, T value) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        _jsonCommands.Set($"{_dataPrefix}{dataId}", "$", cacheValue);
    }


    public void RemoveDataAsync(string key)
    {
        var exists = _cacheDb.KeyExists(key);

        if (exists)
            _cacheDb.KeyDelete(key);
    }
}
