using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleSpamer.CommandHandlers
{
    internal class AdminAcssesedSlashCommand : SlashCommandHandler
    {
        private SlashCommandHandler SlashCommandHandler { get; set; }
        private string ADMIN_USERNAME { get; set; }

        public AdminAcssesedSlashCommand(SlashCommandHandler _slashCommandHandler, string _adminUsername)
        {
            SlashCommandHandler = _slashCommandHandler;
            ADMIN_USERNAME = _adminUsername;
        }

        private AdminAcssesedSlashCommand(ITelegramBotClient _client, DataDbContext _dataDbContext) : base(_client, _dataDbContext)
        {
        }

        public override void Handle(Message _message)
        {
            if (_message.Chat.Username != ADMIN_USERNAME)
            {
                Console.WriteLine(_message.Chat.Username + " tries Forbiden opperation");
                throw new ForbidenException("403 Ты не моя мамочка");
            }
            SlashCommandHandler.Handle(_message);
        }
    }
}
