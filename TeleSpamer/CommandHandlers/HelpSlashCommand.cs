using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleSpamer.CommandHandlers
{
    internal class HelpSlashCommand : SlashCommandHandler
    {
        public HelpSlashCommand(ITelegramBotClient _client, DataDbContext _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            botClient.SendMessage(_message.Chat.Id, dataContext.telegramUsers.Count().ToString());
        }
    }
}
