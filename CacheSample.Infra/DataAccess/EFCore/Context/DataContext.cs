using CacheSample.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CacheSample.Infra.DataAccess.EFCore.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Student> Student { get; set; }
}
