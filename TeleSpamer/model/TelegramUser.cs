using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleSpamer.model
{
    [Table("TelegramUser")]
    [PrimaryKey("Username")]
    internal class TelegramUser
    {
        public string Username { get; set; }
        public long chatId { get; set; }

        public override string ToString()
        {
            return base.ToString() 
                + " = Username: " + Username 
                + "; chatId: " + chatId;
            ;
        }
    }
}
