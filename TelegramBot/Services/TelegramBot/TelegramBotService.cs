using MongoDB.Driver;
using RedditScraper.Helper.Environment.Enums;
using RedditScraper.Services.Environment;
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
        IEnvService envService,
        ILogger<TelegramBotService> logger
    )
    {
        BOT_TOKEN =
            envService.GetEnvVariable(EnvVariableKeys.TELEGRAM_BOT_TOKEN)
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
                    Description =
                        "To get the latest top posts, simply type /new_report. You can choose between Markdown, PDF, or HTML format for your report.",
                },
                new BotCommand()
                {
                    Command = "old_report",
                    Description =
                        "To access previous reports, choose the date you’re interested in. We have data for the past few days available.",
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
        try
        {
            if (update.CallbackQuery != null)
            {
                HandleCallbackQuery(botClient, update, cancellationToken);
                return;
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
        catch (Exception e)
        {
            await botClient.SendMessage(
                update.Message?.Chat,
                $"Oh dear looks like something went wrong",
                cancellationToken: cancellationToken
            );
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
                _logger.LogInformation($"Handling get past day posts");
                _pastDayService.HandleGetPastDayPosts(botClient, update, cancellationToken);
                break;

            case "OldReport":
                _logger.LogInformation($"Handling get old posts");
                _pastDayService.HandleGetOldReport(botClient, update, cancellationToken);
                break;
        }
    }
}
