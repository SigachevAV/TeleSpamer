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
        public StartSlashCommand(ITelegramBotClient _client, SyncRepository _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            Chat chat = _message.Chat;
            TelegramUser telegramUser = new model.TelegramUser { chatId = chat.Id, Username = chat.Username };
            Console.WriteLine(telegramUser);
            syncRepository.AddUserIfNotExist(telegramUser);
            botClient.SendMessage(_message.Chat.Id, "Registred");
        }
    }
}
