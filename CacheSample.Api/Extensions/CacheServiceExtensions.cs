using Microsoft.Extensions.Caching.Memory;

namespace CacheSample.Api.Extensions;

public static class CacheServiceExtensions
{
    public static void AddDistributedMemoryCacheService(this IServiceCollection services, IConfiguration config)
    {
        services.AddDistributedMemoryCache();

        services.AddSingleton<IMemoryCache, MemoryCache>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("redis");
        });
    }
}
