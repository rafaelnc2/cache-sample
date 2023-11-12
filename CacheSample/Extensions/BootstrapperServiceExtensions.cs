using CacheSample.Infra.IoC;

namespace CacheSample.Api.Extensions;

public static class BootstrapperServiceExtensions
{
    public static void AddBootstrapperRegistration(this IServiceCollection services)
    {
        new RootBootstrapper().BootstrapperRegisterServices(services);
    }
}
