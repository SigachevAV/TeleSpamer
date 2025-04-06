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
    internal class RemoveSlashCommand : SlashCommandHandler
    {

        public RemoveSlashCommand(ITelegramBotClient _telegramBotClient, SyncRepository _dataDbContext) : base(_telegramBotClient, _dataDbContext)
        { }
        
        public override void Handle(Message _message)
        {
            string userName;
            try 
            { 
                userName = CommonComponents.ExtractUsername(_message.Text);
            } 
            catch (ValidationException e)
            { 
                botClient.SendMessage(_message.Chat.Id, e.Message);
                return;
            }
            syncRepository.RemoveNotificationsByUsername(userName);
            string response = "Notification removed for " + userName;
            Console.WriteLine(response);
            botClient.SendMessage(_message.Chat.Id, response);
        }
    }
}
