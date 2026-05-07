using Microsoft.EntityFrameworkCore;
using News.Domain;
using News.Domain.Models;
using News.Infrastructure;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace News.AISummarizator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    private string[] _topics;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        await SendTestRequestAsync(stoppingToken);

        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<NewsRepository>();

        _topics = await repo.FetchTopicsFromDBAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var newsItems = await repo.FetchNewsFromDBAsync(5, stoppingToken);

            if (newsItems != null && newsItems.Count > 0)
            {
                await SendSummaryRequestAsync(newsItems, stoppingToken);
                continue;
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private string BuildPrompt(List<NewsItem> newsItems)
    {
        return $@"
            You are a news classifier and summarizer.
            
            TASK:
            For each news item:
            1. Write a short summary (1-2 sentences)
            2. Assign topics with relevance scores (0 to 1)
            
            ALLOWED TOPICS:
            {string.Join(", ", _topics)}
            
            RULES:
            - Return ONLY valid JSON
            - No markdown
            - No explanations
            - Score must be float between 0 and 1
            - Max 3 topics per article
            
            FORMAT:
            {{
              ""results"": [
                {{
                  ""summary"": ""string"",
                  ""topics"": [
                    {{ ""name"": ""string"", ""score"": 0.0 }}
                  ]
                }}
              ]
            }}
            
            NEWS:
            {string.Join("\n\n", newsItems.Select(n =>
                $"""
            TITLE: {n.Title}
            CONTENT: {n.Content}
            """))}
            ";
    }

    private OllamaChatRequest BuildRequest(string prompt)
    {
        return new OllamaChatRequest(
            Model: "llama3.2",
            Messages: [new OllamaMessage("user", prompt)],
            Stream: false
        );
    }

    private async Task EnsureModelReadyAsync(HttpClient http, CancellationToken ct)
    {
        const int retryAttempt = 10;

        for (int i = 0; i < retryAttempt; i++)
        {
            var tags = await http.GetStringAsync("/api/tags", ct);

            if (tags.Contains("llama3.2"))
                return;

            _logger.LogInformation("Model not ready yet. Waiting...");

            await Task.Delay(TimeSpan.FromMinutes(2), ct);
        }

        throw new Exception("Model llama3.2 is not available in Ollama");
    }

    private async Task<OllamaChatResponse?> SendRequestAsync(
    HttpClient http,
    OllamaChatRequest request,
    CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("/api/chat", request, ct);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OllamaChatResponse>(ct);
    }

    private async Task SendSummaryRequestAsync(List<NewsItem> newsItems, CancellationToken ct)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("ollama");

            await EnsureModelReadyAsync(http, ct);

            var prompt = BuildPrompt(newsItems);
            var request = BuildRequest(prompt);

            var result = await SendRequestAsync(http, request, ct);

            _logger.LogInformation(
                "Ollama response: {content}",
                result?.Message?.Content ?? "<empty>");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send request to Ollama");
        }
    }



    private async Task SendTestRequestAsync(CancellationToken ct)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("ollama");

            await EnsureModelReadyAsync(http, ct);

            var request = BuildRequest("Hello. Ready to work?");

            var result = await SendRequestAsync(http, request, ct);

            _logger.LogInformation(
                "Ollama response: {content}",
                result?.Message?.Content ?? "<empty>");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send request to Ollama");
        }
    }
}

// --- DTO ---

public record OllamaChatRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] List<OllamaMessage> Messages,
    [property: JsonPropertyName("stream")] bool Stream
);

public record OllamaMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content
);

public record OllamaChatResponse(
    [property: JsonPropertyName("message")] OllamaMessage? Message
);