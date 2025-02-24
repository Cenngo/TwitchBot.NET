﻿using Microsoft.Extensions.DependencyInjection;
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
using TwitchLib.Client;
using TwitchLib.Client.Events;
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
            _methods = assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0).Where(x => x.DeclaringType.BaseType == typeof(CommandModule));

            var classes = _methods.Select(x => x.DeclaringType.Name).Distinct();

            foreach(var cla in classes)
            {
                Console.WriteLine($"Activated Command Module : {cla}");
            }            
        }

        public void ExecuteChat(CommandContext ctx, IServiceProvider services)
        {
            var method = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Chat).
                First(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.Command.ToLower());

            InvokeMethod(method, services, ctx);
        }

        public void ExecuteWhisper ( CommandContext ctx, IServiceProvider services )
        {
            var method = _methods.Where(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Type == CommandType.Whisper).
                First(x => ( (CommandAttribute)Attribute.GetCustomAttribute(x, typeof(CommandAttribute)) ).Command.ToLower() == ctx.Command.ToLower());

            var obj = ActivatorUtilities.CreateInstance(services, method.DeclaringType);

            ( obj as CommandModule ).CommandContext = ctx;
            var args = new List<object>();

            args.AddRange(ctx.Args);

            try
            {
                method.Invoke(obj, ctx.Args.ToArray());
            }
            catch (Exception)
            {

            }
        }

        private void InvokeMethod(MethodInfo method, IServiceProvider services, CommandContext ctx)
        {
            var obj = ActivatorUtilities.CreateInstance(services, method.DeclaringType);

            ( obj as CommandModule ).CommandContext = ctx;
            var args = new List<object>();

            args.AddRange(ctx.Args);

            try
            {
                method.Invoke(obj, ctx.Args.ToArray());
            }
            catch (Exception)
            {

            }
        }
    }
}
