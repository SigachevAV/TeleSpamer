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
        const string LIST_COMMAND = "/list";


        private Dictionary<string, SlashCommandHandler> handlers = new Dictionary<string, SlashCommandHandler>();

        private TelegramBotClient client;
        private DataDbContext dataContext;
        private SyncRepository syncRepository;
        private Task spammer;
        public TelegramHandler(string _token, DataDbContext _dataDbContext, string adminUsername) 
        {
            this.client = new TelegramBotClient(_token);
            this.dataContext = _dataDbContext;
            this.syncRepository = new SyncRepository(_dataDbContext);

            this.handlers.Add(HELP_COMMAND, new HelpSlashCommand(client, syncRepository));
            this.handlers.Add(START_COMMAND, new StartSlashCommand(client, syncRepository));
            this.handlers.Add(NEW_COMMAND, new AdminAcssesedSlashCommand(new NewSlashCommand(client, syncRepository), adminUsername));
            this.handlers.Add(REMOVE_COMMAND, new AdminAcssesedSlashCommand(new RemoveSlashCommand(client, syncRepository), adminUsername));
            this.handlers.Add(LIST_COMMAND, new AdminAcssesedSlashCommand(new ListSlashCommand(client, syncRepository), adminUsername));
            Console.WriteLine("HandlerConfigured");
        }

        public void Start()
        {
            client.StartReceiving(MessageHandler, ErrorHandler);
            spammer = Task.Run(Spam);
        }

        public void Await()
        {
            spammer.Wait();
        }

        private void Spam()
        {
            while (true)
            {
                try
                {
                    List<TelegramNotification> messages = syncRepository.GetNotificationsForToday();
                    foreach (TelegramNotification notification in messages)
                    {
                        client.SendMessage(syncRepository.FindUser(notification.Username).chatId, notification.message);
                    }
                    Thread.Sleep(24 * 60 * 60 * 1000);
                    //Thread.Sleep(5000);
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex);
                    continue;
                }
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
                case LIST_COMMAND:
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
