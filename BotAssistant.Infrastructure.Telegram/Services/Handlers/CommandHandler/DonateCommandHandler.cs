using Telegram.Bot.Types.Payments;

namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.CommandHandler;

public sealed class DonateCommandHandler : IDonateCommandHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOptions<PaymentOptions> _paymentOptions;

    public DonateCommandHandler(ITelegramBotClient telegramBotClient, IOptions<PaymentOptions> paymentOptions)
    {
        _telegramBotClient = telegramBotClient;
        _paymentOptions = paymentOptions;
    }

    public async Task Handle(Message message)
    {
        await _telegramBotClient.SendInvoiceAsync(message.Chat.Id, "Пожертвование", "На развитие проекта",
            Guid.NewGuid().ToString(), _paymentOptions.Value.ProviderToken,
            "RUB", new LabeledPrice[]
            {
                new LabeledPrice("Пожертвование", 100 * 100),
            });
    }

    public async Task AnswerPreCheckoutQuery(PreCheckoutQuery preCheckoutQuery)
    {
       await _telegramBotClient.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
    }
}
