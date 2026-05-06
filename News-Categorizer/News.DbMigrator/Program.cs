using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using News.Infrastructure;
using Npgsql;

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
var connectionString = config.GetConnectionString("newsdb");

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

Console.WriteLine("Waiting for database...");

var maxRetries = 10;
for (var i = 0; i < maxRetries; i++)
{
    try
    {
        Console.WriteLine($"Attempt {i + 1}/{maxRetries}...");
        db.Database.Migrate();
        Console.WriteLine("Done.");
        break;
    }
    catch (NpgsqlException ex)
    {
        if (i == maxRetries - 1) throw;
        Console.WriteLine($"Failed: {ex.Message}");
        Console.WriteLine("Retrying in 5 seconds...");
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}

Console.WriteLine("Done.");