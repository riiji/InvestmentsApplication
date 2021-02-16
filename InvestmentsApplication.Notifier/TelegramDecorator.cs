using Telegram.Bot;

namespace InvestmentsApplication.Notifier
{
    public class TelegramDecorator : Decorator
    {
        private readonly TelegramBotClient telegramClient = new TelegramBotClient("");
        private readonly string chatId;

        public TelegramDecorator(Notifier notifier, string chatId) : base(notifier)
        {
            this.chatId = chatId;
        }

        public override void Notify(string message)
        {
            base.Notify(message);

            var sendMessageTask = telegramClient.SendTextMessageAsync(chatId, message);
            sendMessageTask.Wait();
        }
    }
}