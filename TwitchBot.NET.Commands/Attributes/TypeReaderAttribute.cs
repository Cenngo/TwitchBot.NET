using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchBot.NET.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class TypeReaderAttribute : Attribute
    {
        public Type Type { get; private set; }

        public TypeReaderAttribute(Type readerType)
        {
            Type = readerType;
        }
    }
}
