using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.NET.Commands.Enums;

namespace TwitchBot.NET.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        private string command;
        private CommandType type;

        public string Command
        {
            get
            {
                return command;
            }
        }

        public CommandType Type
        {
            get
            {
                return type;
            }
        }

        public CommandAttribute ( string command, CommandType type )
        {
            this.command = command;
            this.type = type;
        }
    }
}
