using NRedisStack.Search;

namespace CacheSample.Shared.Interfaces;

public interface ICacheService
{
    public void CreateIndex(Schema schema, string indexName, string dataPrefix);

    IEnumerable<T> GetAllData<T>() where T : class;

    T? GetDataById<T>(int dataId) where T : class;

    void SetData<T>(int dataId, T value) where T : class;

    void RemoveDataAsync(string key);
}
