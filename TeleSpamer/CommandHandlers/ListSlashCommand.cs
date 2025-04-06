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
    internal class ListSlashCommand : SlashCommandHandler
    {
        public ListSlashCommand(ITelegramBotClient _client, SyncRepository _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Найдены следующие оповещения: \n");
            syncRepository.GetAllNotifications()
                .ForEach(i =>
                {
                    sb.Append(i.ToString());
                    sb.Append("\n");
                });
            botClient.SendMessage(_message.Chat.Id, sb.ToString());
            Console.WriteLine(sb.ToString());
        }
    }
}
