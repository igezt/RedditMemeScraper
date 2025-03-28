using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Services.PastDay;

public interface IPastDayService
{
    void HandleRequestPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );

    void HandleGetPastDayPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );

    void HandleRequestOldPosts(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    );
}
