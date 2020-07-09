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
        public string Command { get; private set; }
        public TwitchLibMessage Message { get; private set; }
        public IEnumerable<string> Args { get; private set; }

        public CommandContext(TwitchClient client, ChatCommand command)
        {
            Client = client;
            Message = command.ChatMessage;
            Command = command.CommandText;
            Args = command.ArgumentsAsList;
        }

        public CommandContext(TwitchClient client, WhisperCommand command)
        {
            Client = client;
            Message = command.WhisperMessage;
            Command = command.CommandText;
            Args = command.ArgumentsAsList;
        }
    }
}
