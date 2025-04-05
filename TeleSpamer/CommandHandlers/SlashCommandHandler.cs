using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleSpamer.CommandHandlers
{
    internal abstract class SlashCommandHandler
    {
        protected ITelegramBotClient botClient { get; set; }
        protected DataDbContext dataContext { get; set; }

        public SlashCommandHandler(ITelegramBotClient _client, DataDbContext _dataDbContext) 
        { 
            botClient = _client;
            dataContext = _dataDbContext;
        }

        protected SlashCommandHandler()
        {
        }

        public abstract void Handle(Message _message);
    }
}
