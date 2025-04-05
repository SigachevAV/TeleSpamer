using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TeleSpamer.CommandHandlers;
using TeleSpamer.model;

namespace TeleSpamer
{
    internal class TelegramHandler
    {
        const string START_COMMAND = "/start";
        const string HELP_COMMAND = "/help";
        const string NEW_COMMAND = "/new";
        const string REMOVE_COMMAND = "/remove";


        private Dictionary<string, SlashCommandHandler> handlers = new Dictionary<string, SlashCommandHandler>();

        private TelegramBotClient client;
        private DataDbContext dataContext;
        private Task spammer;
        public TelegramHandler(string _token, DataDbContext _dataDbContext, string adminUsername) 
        {
            this.client = new TelegramBotClient(_token);
            this.dataContext = _dataDbContext;

            this.handlers.Add(HELP_COMMAND, new HelpSlashCommand(client, dataContext));
            this.handlers.Add(START_COMMAND, new StartSlashCommand(client, dataContext));
            this.handlers.Add(NEW_COMMAND, new AdminAcssesedSlashCommand(new NewSlashCommand(client, dataContext), adminUsername));
            this.handlers.Add(REMOVE_COMMAND, new AdminAcssesedSlashCommand(new RemoveSlashCommand(client, dataContext), adminUsername));

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
                    .Where(i => i.day == DateTime.Now.Day)
                    .ToList();
                foreach (TelegramNotification notification in messages) 
                {
                    client.SendMessage(notification.telegramUser.chatId, notification.message);
                }
                //Thread.Sleep(24 * 60 * 60 * 1000);
                Thread.Sleep(5000);
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
                case HELP_COMMAND:
                case NEW_COMMAND:
                case REMOVE_COMMAND:
                {
                    try
                    {
                        handlers[_command].Handle(_message);
                    } catch (ForbidenException e)
                    {
                        client.SendMessage(_message.Chat.Id, e.Message);
                    }
                    break;
                }
                default:
                {
                    client.SendMessage(_message.Chat.Id, "Unknown Command");
                    break;
                }
            }
        }

    }
}
