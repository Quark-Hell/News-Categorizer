using News.TelegramBot;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<TelegramBotClient>(_ =>
{
    var token = builder.Configuration["Telegram:BotToken"]!;
    return new TelegramBotClient(token);
});

var host = builder.Build();
host.Run();
