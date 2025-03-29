using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Reddit;
using RedditScraper.Services.RedditPosts.Models;
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
    private readonly string displayDateFormat = "dd MMM yyyy";

    private static readonly TimeZoneInfo SingaporeTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
        "Singapore Standard Time"
    );

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
            var err = "HandleGetPastDayPosts was called with empty callback query.";
            _logger.LogWarning(err);
            throw new InvalidDataException(err);
        }

        if (selectedFileType is null)
        {
            var err = "HandleGetPastDayPosts was called with null file type.";
            _logger.LogWarning(err);
            throw new InvalidDataException(err);
        }

        if (callbackQuery.Message is null)
        {
            var err = "No original message was detected.";
            _logger.LogWarning(err);
            throw new InvalidDataException(err);
        }

        await botClient.AnswerCallbackQuery(
            callbackQuery.Id,
            $"Fetching the latest top posts from r/memes... Please wait.",
            cancellationToken: cancellationToken
        );

        var posts = await _redditService.GetTopPostsInPastDay("memes", 20);
        var fileType = selectedFileType switch
        {
            "MARKDOWN" => FileType.MARKDOWN,
            "PDF" => FileType.PDF,
            "HTML" => FileType.HTML,
            _ => FileType.PDF,
        };

        var fileName = DateTime.Now.ToString(displayDateFormat);

        var filePath = await _redditService.ConvertToFile(fileType, posts, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileToSend = InputFile.FromStream(fileStream);

        await botClient.SendDocument(
            callbackQuery.Message.Chat,
            fileToSend,
            cancellationToken: cancellationToken
        );

        await botClient.SendMessage(
            callbackQuery.Message.Chat,
            $"Here is your report!",
            cancellationToken: cancellationToken
        );
    }

    public async void HandleRequestOldPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var dates = await _redditService.GetDatesWithTopPostsRegistered(5);

        var buttons = dates.Select<DateTime, IEnumerable<InlineKeyboardButton>>(
            (date) =>

                [
                    InlineKeyboardButton.WithCallbackData(
                        ConvertUtcToSingaporeTime(date).ToString(displayDateFormat),
                        $"OldReport-{ConvertUtcToSingaporeTime(date).ToString(dateFormat)}"
                    ),
                ]
        );

        var fileTypeMessage = "Please select a report to re-generate:";

        // Create inline keyboard with buttons
        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

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

        DateTime singaporeTime = DateTime.ParseExact(
            dateStr,
            "dd/MM/yyyy",
            System.Globalization.CultureInfo.InvariantCulture
        );

        DateTime utcTime = ConvertSingaporeTimeToUtc(singaporeTime);

        await botClient.AnswerCallbackQuery(
            callbackQuery.Id,
            $"Hold tight! A PDF report for {dateStr} is on its way!",
            cancellationToken: cancellationToken
        );

        var posts = (await _redditService.GetTopPostsOnSpecificDay("memes", utcTime)).RedditPosts;

        var fileName = singaporeTime.ToString(displayDateFormat);

        var filePath = await _redditService.ConvertToFile(FileType.PDF, posts, fileName);
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

        await botClient.SendMessage(
            callbackQuery.Message.Chat,
            $"Here is your report from {dateStr}!",
            cancellationToken: cancellationToken
        );
    }

    private static DateTime ConvertUtcToSingaporeTime(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, SingaporeTimeZone);
    }

    private static DateTime ConvertSingaporeTimeToUtc(DateTime singaporeTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(singaporeTime, SingaporeTimeZone);
    }
}
