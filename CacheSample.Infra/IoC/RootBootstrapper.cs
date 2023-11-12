using CacheSample.Infra.IoC.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CacheSample.Infra.IoC;

public class RootBootstrapper
{
    public void BootstrapperRegisterServices(IServiceCollection services)
    {
        new RepositoriesBootstrapper().ChildServiceRegister(services);
    }

}
