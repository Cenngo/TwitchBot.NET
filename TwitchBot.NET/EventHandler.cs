using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using TwitchBot.NET.Commands;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Models;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;

namespace TwitchBot.NET
{
    public class EventHandler
    {
        private readonly TwitchClient _client;
        private readonly CommandHandler _handler;
        private readonly IServiceProvider _services;

        public ConsoleColor LogColor { get; set; } = ConsoleColor.Red;
         

        public EventHandler(TwitchClient client, IServiceProvider services)
        {
            _client = client;
            _services = services;

            Console.Title = Assembly.GetExecutingAssembly().GetName().Name;

            _handler = new CommandHandler();
            _handler.DiscoverModules(Assembly.GetEntryAssembly(), _services);
        }

        public void InstallEventHandler()
        {
            _client.OnConnected += Connected;
            _client.OnChatCommandReceived += ChatCommandRecieved;
            _client.OnWhisperCommandReceived += WhisperCommandReceived;
            _client.OnLog += Log;
        }

        private void WhisperCommandReceived ( object sender, TwitchLib.Client.Events.OnWhisperCommandReceivedArgs e )
        {
            var ctx = new CommandContext(_client, e.Command);
            _handler.ExecuteWhisper(ctx);
        }

        private void Log ( object sender, TwitchLib.Client.Events.OnLogArgs e )
        {
            Console.ForegroundColor = LogColor;
            Console.Write($"[{e.DateTime.ToString("H:mm:ss")}]");
            Console.Write($" <{e.BotUsername}>");
            Console.ResetColor();
            Console.WriteLine($" {e.Data}");
        }

        private void Connected ( object sender, TwitchLib.Client.Events.OnConnectedArgs e )
        { 
            _client.InvokeLog(e.BotUsername, $"Connected : {e.BotUsername}", DateTime.Now);
        }

        private void ChatCommandRecieved ( object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e )
        {
            var ctx = new CommandContext(_client, e.Command);
            _handler.ExecuteChat(ctx);
        }
    }
}
