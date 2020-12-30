using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace TwitchBot.NET.Commands.Models
{
    public class CommandContext
    {
        public TwitchClient Client { get; private set; }
        public ChatCommand ChatCommand { get; private set; }
        public WhisperCommand WhisperCommand { get; private set; }
        public IEnumerable<string> Args { get; private set; }

        public CommandContext(TwitchClient client, ChatCommand command)
        {
            Client = client;
            ChatCommand = command;
            Args = command.ArgumentsAsList;
        }

        public CommandContext(TwitchClient client, WhisperCommand command)
        {
            Client = client;
            WhisperCommand = command;
            Args = command.ArgumentsAsList;
        }
    }
}
