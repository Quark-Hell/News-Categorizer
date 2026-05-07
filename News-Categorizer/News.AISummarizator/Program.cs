using Microsoft.EntityFrameworkCore;
using News.AISummarizator;
using News.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

//Data Base
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("newsdb"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure();
        });
});
builder.Services.AddScoped<NewsRepository>();

// Ollama HTTP client — Aspire пробрасывает адрес через переменную среды
// с именем "services__ollama__http__0" автоматически
builder.Services.AddHttpClient("ollama", client =>
{
    // Aspire резолвит URL по имени сервиса
    var ollamaUrl = builder.Configuration["services__ollama__http__0"]
                    ?? "http://localhost:11434";
    client.BaseAddress = new Uri(ollamaUrl);
    client.Timeout = Timeout.InfiniteTimeSpan;
});

builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddStandardResilienceHandler(options =>
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(3);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(5);

        options.Retry.MaxRetryAttempts = 2;

        options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);
        options.CircuitBreaker.MinimumThroughput = 2;
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
