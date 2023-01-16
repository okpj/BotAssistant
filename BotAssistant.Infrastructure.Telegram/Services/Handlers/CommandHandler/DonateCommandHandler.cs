using BotAssistant.Infrastructure.TelegramBot.Model.Constants;
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

    public async Task HandleAsync(Message message)
    {
        await _telegramBotClient.SendInvoiceAsync(message.Chat.Id, DonateConstants.Title, DonateConstants.Descriptions,
            Guid.NewGuid().ToString(), _paymentOptions.Value.ProviderToken,
            DonateConstants.Currency, new LabeledPrice[]
            {
                new LabeledPrice(DonateConstants.Title, 100 * 100),
            });
    }

    public async Task AnswerPreCheckoutQuery(PreCheckoutQuery preCheckoutQuery) =>
        await _telegramBotClient.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
}
