using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchBot.NET.Commands.Interfaces
{
    public interface ITypeReader
    {
        object Convert ( string variable );
    }
}
