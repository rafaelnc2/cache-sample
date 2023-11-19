using CacheSample.Application;

namespace CacheSample.Api.Extensions;

public static class MediatrServiceExtensions
{
    public static void AddMediatrService(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AssemblyRegister>();
        });
    }
}
