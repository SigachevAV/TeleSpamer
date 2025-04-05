using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TeleSpamer.model;

namespace TeleSpamer
{
    internal static class CommonComponents
    {
        public static string ExtractUsername(string _messageText)
        {
            try
            {
                return Regex.Match(_messageText, "@[a-zA-Z0-9]*").Value.Substring(1);
            }
            catch (Exception e)
            {
                throw new ValidationException("invalid Username");
            }
        }
    }
}
