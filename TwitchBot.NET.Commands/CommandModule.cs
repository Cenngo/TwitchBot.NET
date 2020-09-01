﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Reply(string channel, string message, bool dryRun = false)
        {
            CommandContext.Client.SendMessage(channel, message, dryRun);
        }

        public void Reply(JoinedChannel channel, string message, bool dryRun = false )
        {
            CommandContext.Client.SendMessage(channel, message, dryRun);
        }
        
        public void SetContext(CommandContext ctx)
        {
            CommandContext = ctx;
        }
    }
}
