using RedditScraper.Services.Reddit;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services.TelegramBot;

namespace TelegramBot.Services.TelegramBot;

public class TelegramBotService : ITelegramBotService
{
    private readonly string BOT_TOKEN;
    private readonly TelegramBotClient BotClient;
    private readonly IRedditService _redditService;

    // Constructor for the Program class
    public TelegramBotService(IRedditService redditService)
    {
        BOT_TOKEN =
            Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")
            ?? throw new InvalidOperationException("There is no telegram bot token present");
        BotClient = new TelegramBotClient(BOT_TOKEN);
        _redditService = redditService;
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
                    Command = "/report",
                    Description = "Returns a report of the top 20 posts from r/memes from reddit",
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
            // Handle the callback query when a button is pressed
            var callbackQuery = update.CallbackQuery;
            var selectedFileType = callbackQuery.Data; // This will be the callback data (MARKDOWN, PDF, HTML)

            // Respond with a message
            await botClient.AnswerCallbackQuery(
                callbackQuery.Id,
                $"You selected {selectedFileType}",
                cancellationToken: cancellationToken
            );

            // Now, based on the selected file type, you can process further (like fetching Reddit posts and sending the file)
            var posts = await _redditService.GetTopPostsInPastDay("memes", 20);
            var fileType = selectedFileType switch
            {
                "MARKDOWN" => RedditScraper.Services.Adapters.Models.Enums.FileType.MARKDOWN,
                "PDF" => RedditScraper.Services.Adapters.Models.Enums.FileType.PDF,
                "HTML" => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML,
                _ => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML, // Default to HTML
            };

            var filePath = _redditService.ConvertToFile(fileType, posts);
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileToSend = InputFile.FromStream(fileStream);
            await botClient.SendDocument(
                callbackQuery.Message.Chat,
                fileToSend,
                cancellationToken: cancellationToken
            );
        }

        if (update.Message?.Text?.ToLower() == "/report")
        {
            var fileTypeMessage = "Please select a file type:";

            // Create inline keyboard with buttons
            var inlineKeyboard = new InlineKeyboardMarkup(
                [
                    [
                        InlineKeyboardButton.WithCallbackData("Markdown", "ReportPastDay-MARKDOWN"),
                        InlineKeyboardButton.WithCallbackData("PDF", "PDF"),
                        InlineKeyboardButton.WithCallbackData("HTML", "HTML"),
                    ],
                ]
            );

            // Send the message with the inline keyboard
            await botClient.SendMessage(
                update.Message.Chat,
                fileTypeMessage,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken
            );

            // var choice = update.Message?.Text?.Trim(); // User's choice of file type

            // var fileType = choice switch
            // {
            //     "1" => RedditScraper.Services.Adapters.Models.Enums.FileType.MARKDOWN,
            //     "2" => RedditScraper.Services.Adapters.Models.Enums.FileType.PDF, // Replace 'Other' with your actual file type
            //     "3" => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML, // Default to Markdown if choice is invalid
            //     _ => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML, // Default to Markdown if choice is invalid
            // };

            // var posts = await _redditService.GetTopPostsInPastDay("memes", 20); // Call the method
            // var filePath = _redditService.ConvertToFile(fileType, posts);

            // using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // var fileToSend = InputFile.FromStream(fileStream); // Path to file
            // await BotClient.SendDocument(
            //     update.Message.Chat,
            //     fileToSend,
            //     cancellationToken: cancellationToken
            // );
        }
    }

    // Handler for errors
    private Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}
