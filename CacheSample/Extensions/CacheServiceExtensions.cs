﻿using CacheSample.Infra.Caching;
using CacheSample.Shared.Interfaces;

namespace CacheSample.Api.Extensions;

public static class CacheServiceExtensions
{
    public static void AddDistributedMemoryCacheService(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();
    }
}