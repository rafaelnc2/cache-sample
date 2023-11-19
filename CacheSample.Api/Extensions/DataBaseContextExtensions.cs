using CacheSample.Infra.DataAccess.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace CacheSample.API.Extensions;

public static class DataBaseContextExtensions
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration config) =>
        services.AddDbContext<DataContext>(opt => opt.UseSqlServer(config.GetConnectionString("defaultConnection")));
}
