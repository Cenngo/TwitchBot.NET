using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.NET.Commands.Interfaces;
using TwitchBot.NET.Commands.Models;
using TwitchLib.Api.Core.RateLimiter;
using TwitchLib.Client.Models;

namespace TwitchBot.NET.Commands
{
    public abstract class CommandModule : ICommandModule
    {
        public CommandContext CommandContext { get; internal set; }

        protected void Reply(string channel, string message, bool dryRun = false)
        {
            CommandContext.Client.SendMessage(channel, message, dryRun);
        }

        protected void Reply(JoinedChannel channel, string message, bool dryRun = false )
        {
            CommandContext.Client.SendMessage(channel, message, dryRun);
        }
    }
}
