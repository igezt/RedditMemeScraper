using MongoDB.Driver;
using RedditScraper.Services.Reddit;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services.PastDay;
using TelegramBot.Services.TelegramBot;

namespace TelegramBot.Services.TelegramBot;

public class TelegramBotService : ITelegramBotService
{
    private readonly string BOT_TOKEN;
    private readonly TelegramBotClient BotClient;
    private readonly IPastDayService _pastDayService;
    private readonly ILogger<TelegramBotService> _logger;

    // Constructor for the Program class
    public TelegramBotService(
        IRedditService redditService,
        IPastDayService pastDayService,
        ILogger<TelegramBotService> logger
    )
    {
        BOT_TOKEN =
            Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")
            ?? throw new InvalidOperationException("There is no telegram bot token present");
        BotClient = new TelegramBotClient(BOT_TOKEN);
        _pastDayService = pastDayService;
        _logger = logger;
    }

    public async Task Run()
    {
        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };

        // Access BotClient instance here
        await BotClient.SetMyCommands(
            [
                new BotCommand()
                {
                    Command = "new_report",
                    Description = "Returns a report of the top 20 posts from r/memes from reddit",
                },
                new BotCommand()
                {
                    Command = "old_report",
                    Description =
                        "Returns a report that was already generated of the top 20 posts from r/memes from reddit",
                },
            ]
        );
        BotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);
        Console.ReadLine();
        cts.Cancel();
    }

    // Handler for messages
    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        if (update.CallbackQuery != null)
        {
            HandleCallbackQuery(botClient, update, cancellationToken);
        }
        var command = update.Message?.Text?.ToLower();
        switch (command)
        {
            case "/new_report":
                _pastDayService.HandleRequestPastDayPosts(botClient, update, cancellationToken);
                break;
            case "/old_report":
                _pastDayService.HandleRequestOldPosts(botClient, update, cancellationToken);
                break;
            default:
                _logger.LogWarning($"An unidentified command ({command}) was sent in");
                break;
        }
    }

    // Handler for errors
    private Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation($"Error: {exception.Message}");
        return Task.CompletedTask;
    }

    private async void HandleCallbackQuery(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        // Handle the callback query when a button is pressed
        var callbackQuery = update.CallbackQuery;
        var callbackQueryType = callbackQuery.Data.Split("-").First();

        switch (callbackQueryType)
        {
            case "NewReport":
                _pastDayService.HandleGetPastDayPosts(botClient, update, cancellationToken);
                break;

            case "OldReport":
                _pastDayService.HandleGetPastDayPosts(botClient, update, cancellationToken);
                break;
        }
    }
}
