﻿using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.NET.Commands.Models;
using TwitchLib.Client.Models;

namespace TwitchBot.NET.Commands.Interfaces
{
    public interface ICommandModule
    {
        CommandContext CommandContext { get; }

        void Reply ( JoinedChannel channel, string message, bool dryRun );
        void Reply ( string channel, string message, bool dryRun );
        void SetContext ( CommandContext ctx );
    }
}
