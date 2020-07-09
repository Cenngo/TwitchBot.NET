using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.NET.Commands.Interfaces;
using TwitchBot.NET.Commands.Models;

namespace TwitchBot.NET.Commands
{
    public abstract class CommandModule : ICommandModule
    {
        public CommandContext CommandContext { get; protected set; }
    }
}
