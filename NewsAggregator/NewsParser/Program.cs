using NewsParser;
using NewsParser.Aggregator;
using NewsParser.ContentExtractor;
using NewsParser.ContentLoader;
using NewsParser.Parses;
using NewsParser.RSS;
using NewsParser.Sources;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();

// Sources
builder.Services.AddSingleton<INewsSource, HabrSource>();
builder.Services.AddSingleton<INewsSource, RBKSource>();
builder.Services.AddSingleton<INewsSource, MeduzaSource>();

// Content Extractor
builder.Services.AddSingleton<HabrContentExtractor>();
builder.Services.AddSingleton<RBKContentExtractor>();
builder.Services.AddSingleton<MeduzaContentExtractor>();

builder.Services.AddSingleton<IContentExtractorFactory, ContentExtractorFactory>();
builder.Services.AddHttpClient<IContentLoader, HtmlContentLoader>(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
});

// Parser
builder.Services.AddSingleton<INewsParser, DefaultNewsParser>();

// Infrastructure
builder.Services.AddHttpClient<RssClient>();

// Core
builder.Services.AddSingleton<NewsAggregator>();

// Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
