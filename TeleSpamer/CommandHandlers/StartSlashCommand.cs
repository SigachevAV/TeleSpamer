using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TeleSpamer.model;

namespace TeleSpamer.CommandHandlers
{
    internal class StartSlashCommand : SlashCommandHandler
    {
        public StartSlashCommand(ITelegramBotClient _client, DataDbContext _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            Chat chat = _message.Chat;
            if (dataContext.telegramUsers.Find(chat.Username) == null)
            {
                TelegramUser telegramUser = new model.TelegramUser { chatId = chat.Id, Username = chat.Username };
                dataContext.Add(telegramUser);
                Console.WriteLine(telegramUser);
            }
            dataContext.SaveChanges();
        }
    }
}
