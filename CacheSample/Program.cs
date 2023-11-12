using CacheSample.Api.Extensions;
using CacheSample.Api.Filters;
using CacheSample.Infra.DataAccess.EFCore.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalExceptionHandlingFilter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

//builder.Services.AddScoped<GlobalExceptionHandlingFilter>();


builder.Services.AddBootstrapperRegistration();
builder.Services.AddMediatrService();
builder.Services.AddDistributedMemoryCacheService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();