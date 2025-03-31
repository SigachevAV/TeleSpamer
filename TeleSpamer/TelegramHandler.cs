using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TeleSpamer.model;

namespace TeleSpamer
{
    internal class TelegramHandler
    {
        const string START_COMMAND = "/start";

        private TelegramBotClient client;
        private DataDbContext dataContext;
        private Task spammer;
        public TelegramHandler(string _token, DataDbContext _dataDbContext) 
        {
            this.client = new TelegramBotClient(_token);
            this.dataContext = _dataDbContext; 
        }

        public void Start()
        {
            client.StartReceiving(MessageHandler, ErrorHandler);
            spammer = Task.Run(Spam);
        }

        private void Spam()
        {
            while (true)
            {
                List<TelegramNotification> messages = dataContext.telegramNotifications
                    .Where(i => i.day == new DateOnly().Day)
                    .ToList();
                foreach (TelegramNotification notification in messages) 
                {
                    client.SendMessage(notification.Username, notification.message);
                }
                Thread.Sleep(24 * 60 * 60 * 1000);
            }

        }

        private Task MessageHandler(ITelegramBotClient _client, Update _update, CancellationToken _token)
        {
            Message message = _update.Message;
            string command = Regex.Match(message.Text, "/[a-zA-Z]*").Value;
            Disptch(command, message);
            return Task.CompletedTask;
        }

        private Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private void Disptch(string _command, Message _message)
        {
            switch (_command)
            {
                case START_COMMAND:
                {
                    Chat chat = _message.Chat;
                    if(dataContext.telegramUsers.Find(chat.Username) == null)
                    {
                        TelegramUser telegramUser = new model.TelegramUser { chatId = chat.Id, Username = chat.Username };
                        dataContext.Add(telegramUser);
                        Console.WriteLine(telegramUser);
                    }
                    break;
                }
                case "/help":
                {
                    client.SendMessage(_message.Chat.Id, "Kind of helpfull");
                    break;
                }
                case "/new":
                {
                    TelegramNotification notification;
                    try
                    {
                        notification = GetTelegramNotification(_message);
                    }
                    catch (ValidationException ex)
                    {
                        Console.WriteLine(ex.Message);
                        client.SendMessage(_message.Chat.Id, ex.Message);
                        break;
                    }
                    if (dataContext.telegramUsers.Find(notification.Username) == null)
                    {
                        client.SendMessage(_message.Chat.Id, "UnknownUser");
                        break;
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
                    break;
                }
            }
            dataContext.SaveChanges();
        }

        private TelegramNotification GetTelegramNotification(Message _message)
        {
            TelegramNotification telegramNotification = new TelegramNotification();
            try
            {
                telegramNotification.Username = Regex.Match(_message.Text, "@[a-zA-Z0-9]*").Value.Substring(1);
            }
            catch (Exception e) 
            {
                throw new ValidationException("invalid Username");
            }

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
                telegramNotification.message = Regex.Match(_message.Text, "m=.*").Value;
            }
            catch (Exception e)
            {
                throw new ValidationException("invalid Message");
            }

            return telegramNotification;
        }
    }
}
