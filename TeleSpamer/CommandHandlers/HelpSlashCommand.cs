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

        private readonly string HELP = """
            Это бот который позволяет администратору слать оповещения.
            /help - выводит эту справку
            /start - выдаёт боту право отправлять вам сообщения.
            /list - показывает список всех оповещений которые посылает бот.
            /remove @<TelegramTag> - отменяет отправку оповещения
            /new @<TelegramTag> d=<день Отправки> m=<текст сообщения>
            """;

        public HelpSlashCommand(ITelegramBotClient _client, SyncRepository _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            botClient.SendMessage(_message.Chat.Id, HELP);
        }
    }
}
