using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using TeleSpamer.model;

namespace TeleSpamer.CommandHandlers
{
    internal class NewSlashCommand : SlashCommandHandler
    {
        public NewSlashCommand(ITelegramBotClient _client, DataDbContext _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            TelegramNotification notification;
            try
            {
                notification = GetTelegramNotification(_message);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
                botClient.SendMessage(_message.Chat.Id, ex.Message);
                return;
            }
            if (dataContext.telegramUsers.Find(notification.Username) == null)
            {
                botClient.SendMessage(_message.Chat.Id, "UnknownUser");
                return;
            }


            if (dataContext.telegramNotifications.Find(notification.Username) == null)
            {
                dataContext.telegramNotifications.Add(notification);
            }
            else
            {
                TelegramNotification updating = dataContext.telegramNotifications.Find(notification.Username);
                updating.day = notification.day;
                updating.message = notification.message;
            }

            Console.WriteLine(notification);
            dataContext.SaveChanges();
        }

        private TelegramNotification GetTelegramNotification(Message _message)
        {
            TelegramNotification telegramNotification = new TelegramNotification();

            telegramNotification.Username = CommonComponents.ExtractUsername(_message.Text);    
            try
            {
                int day = int.Parse(Regex.Match(_message.Text, "d=\\d{1,2}").Value.Substring(2));
                if (day < 1 || day > 28)
                {
                    throw new ValidationException("Дата должны быть между 1 и 29");
                }
                telegramNotification.day = day;
            }
            catch (ValidationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException("invalid Date");
            }

            try
            {
                telegramNotification.message = Regex.Match(_message.Text, "m=.*").Value.Substring(2);
            }
            catch (Exception e)
            {
                throw new ValidationException("invalid Message");
            }

            return telegramNotification;
        }

    }
}
