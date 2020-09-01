using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Interfaces;

namespace TwitchBot.NET.Commands.TypeReaders
{
    [TypeReader(typeof(int))]
    internal class IntegerTypeReader : ITypeReader
    {
        public object Convert ( string variable )
        {
            return System.Convert.ToInt32(variable);
        }
    }
}
