using System;

namespace TwitchBot.NET
{
    class Program
    {
        static void Main ( string[] args )
        {
            var bot = new Bot("lemetallicat", "jzeym90mjxrlyjlmlzos97e0x2ggsg", "leMetallicat", null, new char[] { '!', '>', '-'});
            bot.ConnectAsync().GetAwaiter().GetResult();
        }
    }
}
