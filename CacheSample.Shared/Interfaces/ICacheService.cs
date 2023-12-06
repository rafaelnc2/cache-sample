namespace CacheSample.Shared.Interfaces;

public interface ICacheService
{
    Task<IEnumerable<T>> GetAllDataAsync<T>(string hashKey) where T : class;

    Task<T?> GetDataByIdAsync<T>(string hashey, int dataId) where T : class;

    Task SetDataAsync<T>(string hashey, int dataId, T value) where T : class;

    Task RemoveDataAsync(string key);
}
