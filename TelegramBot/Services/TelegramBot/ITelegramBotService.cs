namespace TelegramBot.Services.TelegramBot;

/// <summary>
/// Interface representing the core functionality of a Telegram Bot Service.
/// This interface defines the methods required to interact with the Telegram Bot API,
/// including starting the bot and handling user interactions.
/// </summary>
public interface ITelegramBotService
{
    /// <summary>
    /// Starts the bot and begins receiving updates from the Telegram server.
    /// This method will initialize the necessary components and begin processing commands
    /// and messages received from users. The bot will continue running and listening for updates
    /// until the process is terminated.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of starting the bot.
    /// </returns>
    Task Run();
}
