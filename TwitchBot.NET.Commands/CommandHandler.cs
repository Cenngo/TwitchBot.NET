using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Enums;
using TwitchBot.NET.Commands.Interfaces;
using TwitchBot.NET.Commands.Models;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace TwitchBot.NET.Commands
{
    public class CommandHandler
    {
        private IEnumerable<MethodInfo> _methods;

        public CommandHandler()
        {

        }

        public void DiscoverModules(Assembly assembly)
        {
            _methods = assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0);

            var classes = _methods.Select(x => x.DeclaringType.Name).Distinct();

            foreach(var cla in classes)
            {
                Console.WriteLine($"Activated {cla}");
            }
        }

        public void ExecuteChat(CommandContext ctx)
        {
            var method = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Chat).
                First(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.Command.ToLower());

            var obj = Activator.CreateInstance(method.DeclaringType);
            var args = new List<object>();

            args.Add(ctx);
            args.AddRange(ctx.Args);

            try
            {
                method.Invoke(obj, args.ToArray());
            }
            catch (Exception)
            {
                
            }
        }

        public void ExecuteWhisper ( CommandContext ctx )
        {
            var method = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Whisper).
                First(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.Command.ToLower());

            var obj = Activator.CreateInstance(method.DeclaringType);
            var args = new List<object>();

            if (method.DeclaringType !is ICommandModule)
                return;

            args.Add(ctx);
            args.AddRange(ctx.Args);

            try
            {
                method.Invoke(obj, args.ToArray());
            }
            catch
            {
                 
            }
        }
    }
}
