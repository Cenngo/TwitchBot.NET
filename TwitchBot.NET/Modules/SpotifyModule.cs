using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchBot.NET.Commands;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Enums;
using TwitchBot.NET.Commands.Models;

namespace TwitchBot.NET.Modules
{
    public class SpotifyModule : CommandModule
    {
        [Command("play", CommandType.Chat)]
        public void Play(CommandContext ctx, string message)
        {
            ctx.Client.SendMessage("leMetallicat", message, false);
            Console.WriteLine("Play");
        }

        [Command("stop", CommandType.Chat)]
        public void Stop( CommandContext ctx, string message )
        {
            ctx.Client.SendMessage("leMetallicat", message, false);
            Console.WriteLine("Stop");
        }
    }
}
