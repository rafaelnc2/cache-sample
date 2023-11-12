using CacheSample.Core.Repositories;
using CacheSample.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CacheSample.Infra.IoC.Repositories;

public class RepositoriesBootstrapper
{
    public void ChildServiceRegister(IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();
    }
}
