using Microsoft.Extensions.Caching.Memory;

namespace CacheSample.Api.Extensions;

public static class CacheServiceExtensions
{
    public static void AddDistributedMemoryCacheService(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.AddSingleton<IMemoryCache, MemoryCache>();
    }
}
