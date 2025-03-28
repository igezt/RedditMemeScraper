using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Reddit;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Services.PastDay;

public class PastDayService(ILogger<PastDayService> logger, IRedditService redditService)
    : IPastDayService
{
    private readonly IRedditService _redditService = redditService;
    private readonly ILogger<PastDayService> _logger = logger;

    private readonly string dateFormat = "dd/MM/yyyy";

    public async void HandleRequestPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var fileTypeMessage = "Please select a file type:";

        // Create inline keyboard with buttons
        var inlineKeyboard = new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("Markdown", "NewReport-MARKDOWN"),
                    InlineKeyboardButton.WithCallbackData("PDF", "NewReport-PDF"),
                    InlineKeyboardButton.WithCallbackData("HTML", "NewReport-HTML"),
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
    }

    public async void HandleGetPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var callbackQuery = update.CallbackQuery;

        var selectedFileType = callbackQuery?.Data?.Split("-")[1];

        if (callbackQuery is null)
        {
            _logger.LogWarning("HandleGetPastDayPosts was called with empty callback query.");
            return;
        }

        if (selectedFileType is null)
        {
            _logger.LogWarning("HandleGetPastDayPosts was called with null file type.");
            return;
        }

        await botClient.AnswerCallbackQuery(
            callbackQuery.Id,
            $"You selected {selectedFileType}",
            cancellationToken: cancellationToken
        );

        var posts = await _redditService.GetTopPostsInPastDay("memes", 20);
        var fileType = selectedFileType switch
        {
            "MARKDOWN" => RedditScraper.Services.Adapters.Models.Enums.FileType.MARKDOWN,
            "PDF" => RedditScraper.Services.Adapters.Models.Enums.FileType.PDF,
            "HTML" => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML,
            _ => RedditScraper.Services.Adapters.Models.Enums.FileType.HTML, // Default to HTML
        };

        var filePath = await _redditService.ConvertToFile(fileType, posts);
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileToSend = InputFile.FromStream(fileStream);

        if (callbackQuery.Message is null)
        {
            _logger.LogWarning("No original message was detected.");
            return;
        }

        await botClient.SendDocument(
            callbackQuery.Message.Chat,
            fileToSend,
            cancellationToken: cancellationToken
        );
    }

    public async void HandleRequestOldPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var dates = await _redditService.GetDatesWithTopPostsRegistered(10);

        var buttons = dates.Select(
            (date) =>
                InlineKeyboardButton.WithCallbackData(
                    date.ToString(dateFormat),
                    $"OldReport-{date.ToString(dateFormat)}"
                )
        );

        var fileTypeMessage = "Please select a report to re-generate:";

        // Create inline keyboard with buttons
        var inlineKeyboard = new InlineKeyboardMarkup([buttons]);

        // Send the message with the inline keyboard
        await botClient.SendMessage(
            update.Message.Chat,
            fileTypeMessage,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );
    }

    public async void HandleGetOldReport(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var callbackQuery = update.CallbackQuery;

        var dateStr = callbackQuery?.Data?.Split("-")[1];

        if (callbackQuery is null)
        {
            _logger.LogWarning("HandleGetOldReport was called with empty callback query.");
            return;
        }

        if (dateStr is null)
        {
            _logger.LogWarning("HandleGetOldReport was called with null date.");
            return;
        }

        DateTime parsedDate = DateTime.ParseExact(
            dateStr,
            "dd/MM/yyyy",
            System.Globalization.CultureInfo.InvariantCulture
        );

        await botClient.AnswerCallbackQuery(
            callbackQuery.Id,
            $"You selected {dateStr}",
            cancellationToken: cancellationToken
        );

        var posts = (
            await _redditService.GetTopPostsOnSpecificDay("memes", parsedDate)
        ).RedditPosts;

        var filePath = await _redditService.ConvertToFile(FileType.HTML, posts);
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileToSend = InputFile.FromStream(fileStream);

        if (callbackQuery.Message is null)
        {
            _logger.LogWarning("No original message was detected.");
            return;
        }

        await botClient.SendDocument(
            callbackQuery.Message.Chat,
            fileToSend,
            cancellationToken: cancellationToken
        );
    }
}
