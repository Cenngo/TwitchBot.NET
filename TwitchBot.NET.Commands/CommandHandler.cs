using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Enums;
using TwitchBot.NET.Commands.Interfaces;
using TwitchBot.NET.Commands.Models;
using TwitchLib.Api.Core.Extensions.System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchBot.NET.Commands
{
    public class CommandHandler
    {
        private IEnumerable<MethodInfo> _methods;
        private IDictionary<Type,object> _instances = new Dictionary<Type, object>();
        private IEnumerable<ITypeReader> _typeReaders;

        public CommandHandler()
        {
        }

        public void DiscoverModules(Assembly assembly, IServiceProvider services)
        {
            _methods = assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0).Where(x => x.DeclaringType.BaseType == typeof(CommandModule));

            var classes = _methods.Select(x => x.DeclaringType).Distinct();

            foreach(var cla in classes)
            {
                var instance = ActivatorUtilities.CreateInstance(services, cla);
                _instances.Add(cla ,instance);

                Console.WriteLine($"Activated Command Module : {cla.Name}");
            }            
        }

        public void ExecuteChat(CommandContext ctx)
        {
            if (ctx.ChatCommand == null)
                return;

            var methods = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Chat).
            Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.ChatCommand.CommandText.ToLower());

            if (methods.Any())
                InvokeMethod(methods.First(), ctx);
            else
                throw new Exception("There is no matching command method.");
        }

        public void ExecuteWhisper ( CommandContext ctx)
        {
            if (ctx.WhisperCommand == null)
                return;

            var methods = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Whisper).
            Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.WhisperCommand.CommandText.ToLower());

            if (methods.Any())
                InvokeMethod(methods.First(), ctx);
            else
                throw new Exception("There is no matching command method.");
        }

        private void InvokeMethod(MethodInfo method, CommandContext ctx)
        {
            var declaring = method.DeclaringType;
            if (!_instances.ContainsKey(declaring))
                return;

            var cla = _instances.First(x => x.Key == declaring).Value as CommandModule;
            cla.CommandContext = ctx;

            var parameters = method.GetParameters();
            if (ctx.Args.Count() < parameters.Count())
                return;
            
            var args = new object[parameters.Count()];

            foreach (var parameter in parameters)
            {
                var position = parameter.Position;
                if (parameter.GetCustomAttributes(typeof(ParamArrayAttribute)).Any())
                {
                    var arr = ctx.Args.ToList().GetRange(position, ctx.Args.Count() - position).ToArray();
                    args[position] = arr;
                    break;
                }
                else
                {

                    args[position] = ctx.Args.ElementAt(position);
                }
            }

            try
            {
                method.Invoke(cla, args.ToArray());
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
