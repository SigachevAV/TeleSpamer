using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleSpamer.model
{
    [Table("TelegramNotification")]
    [PrimaryKey("Username")]
    internal class TelegramNotification
    {
        public int day { get; set; }
        public string message { get; set; }
        //link
        public TelegramUser telegramUser { get; set; }

        [ForeignKey("telegramUser")]
        public string Username { get; set; }

        public override string ToString()
        {
            return base.ToString()
                + " day: " + this.day.ToString()
                + " message: " + this.message
                + " Username: " + this.Username;
        }
    }
}
