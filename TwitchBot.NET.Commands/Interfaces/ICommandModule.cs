using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.NET.Commands.Models;

namespace TwitchBot.NET.Commands.Interfaces
{
    public interface ICommandModule
    {
        CommandContext CommandContext { get; }
    }
}
