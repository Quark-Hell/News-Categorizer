using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace News.TelegramBot;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly TelegramBotClient _bot;

    public Worker(ILogger<Worker> logger, TelegramBotClient bot)
    {
        _logger = logger;
        _bot = bot;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = []
        };

        _bot.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);

        var me = await _bot.GetMe(stoppingToken);

        Console.WriteLine($"Bot @{me.Username} started");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient bot,
        Update update,
        CancellationToken ct)
    {
        if (update.Type != UpdateType.Message)
            return;

        var message = update.Message;

        if (message?.Text == null)
            return;

        if (message.Text == "/start")
        {
            await bot.SendMessage(
                message.Chat.Id,
                "Hello! News bot is running.",
                cancellationToken: ct);
        }
    }

    private Task HandleErrorAsync(
    ITelegramBotClient bot,
    Exception ex,
    CancellationToken ct)
    {
        Console.WriteLine(ex);

        return Task.CompletedTask;
    }
}
