using CacheSample.Infra.Caching;
using CacheSample.Shared.Interfaces;
using StackExchange.Redis;

namespace CacheSample.Api.Extensions;

public static class CacheServiceExtensions
{
    public static void AddRedisService(this IServiceCollection services, IConfiguration config)
    {
        string redisConnectionString = config.GetConnectionString("redis") ?? string.Empty;

        services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddScoped<ICacheService, CacheService>();
    }
}
