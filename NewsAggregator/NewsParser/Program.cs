using NewsParser;
using NewsParser.Aggregator;
using NewsParser.Interfaces;
using NewsParser.Parses;
using NewsParser.RSS;
using NewsParser.Sources;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();

// Sources
builder.Services.AddSingleton<INewsSource, HabrSource>();
builder.Services.AddSingleton<INewsSource, RBKSource>();
builder.Services.AddSingleton<INewsSource, MeduzaSource>();

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
