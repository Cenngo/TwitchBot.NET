using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchBot.NET
{
    public class Bot
    {
        private readonly TwitchClient _client;

        public Bot ( string username, string token, string channel, ClientOptions? options, char[] chatPrefixes = null, char[] whisperPrefixes = null, bool duplicateChatPrefixes = false )
        {
            var credentials = new ConnectionCredentials(username, token);
            var _options = options ?? new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            var wsClient = new WebSocketClient(_options);
            _client = new TwitchClient(wsClient);
            _client.Initialize(credentials, channel);

            if(chatPrefixes != null)
            {
                foreach (var prefix in chatPrefixes)
                {
                    _client.AddChatCommandIdentifier(prefix);

                    if (duplicateChatPrefixes)
                        _client.AddWhisperCommandIdentifier(prefix);
                }
            }

            if(whisperPrefixes != null)
            {
                foreach (var prefix in whisperPrefixes)
                {
                    _client.AddWhisperCommandIdentifier(prefix);
                }
            }

            var serviceManager = new ServiceManager(_client);
            var services = serviceManager.BuildServiceProvider();

            var eventHandler = new EventHandler(_client, services);
            eventHandler.LogColor = ConsoleColor.Blue;
            eventHandler.InstallEventHandler();
        }

        public async Task ConnectAsync()
        {
            _client.Connect();
            await Task.Delay(-1);
        }
    }
}
