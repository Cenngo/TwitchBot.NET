using SpotifyAPI.Web;
using System;

namespace TwitchBot.NET
{
    public class Program
    {
        static void Main ( string[] args )
        {
            var bot = new Bot("lemetallicat", "jzeym90mjxrlyjlmlzos97e0x2ggsg", "leMetallicat", null, new char[] { '!', '>', '-'}, null);
            bot.ConnectAsync().GetAwaiter().GetResult();
        }
    }
}
