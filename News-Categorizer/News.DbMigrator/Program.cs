using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using News.Infrastructure;

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
var connectionString = config.GetConnectionString("Postgres");

var services = new ServiceCollection();

services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsql =>
    {
        npgsql.EnableRetryOnFailure();
    });
});

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("Applying migrations...");

db.Database.Migrate();

Console.WriteLine("Done.");