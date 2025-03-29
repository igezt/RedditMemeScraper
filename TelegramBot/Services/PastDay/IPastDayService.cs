using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Services.PastDay;

/// <summary>
/// Interface for handling past-day posts and old reports in the Telegram Bot service.
/// This interface defines methods for processing user interactions related to past day posts
/// and retrieving old reports through the Telegram Bot.
/// </summary>
public interface IPastDayService
{
    /// <summary>
    /// Handles the user's request to view past day posts.
    /// This method sends an inline keyboard to the user with options to select the file type
    /// for downloading past day posts.
    /// </summary>
    /// <param name="botClient">The Telegram Bot client used to send messages.</param>
    /// <param name="update">The incoming update containing the user's request.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    void HandleRequestPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Handles the retrieval of past day posts and sends the selected file to the user.
    /// This method processes the selected file type (e.g., Markdown, PDF, HTML) and generates
    /// a report of the top posts from the past day before sending the file to the user.
    /// </summary>
    /// <param name="botClient">The Telegram Bot client used to send messages.</param>
    /// <param name="update">The incoming update containing the user's selection.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    void HandleGetPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Handles the user's request to view a list of previously generated reports.
    /// This method retrieves the available dates for old reports and presents the user
    /// with an inline keyboard to select a specific report.
    /// </summary>
    /// <param name="botClient">The Telegram Bot client used to send messages.</param>
    /// <param name="update">The incoming update containing the user's request.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    void HandleRequestOldPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Handles the retrieval of an old report based on the user's selection.
    /// This method fetches the report corresponding to the selected date, generates the
    /// requested file, and sends it to the user.
    /// </summary>
    /// <param name="botClient">The Telegram Bot client used to send messages.</param>
    /// <param name="update">The incoming update containing the user's selection.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    void HandleGetOldReport(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );
}
