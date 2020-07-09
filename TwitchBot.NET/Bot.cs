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
        private readonly EventHandler _handler;

        public Bot ( string username, string token, string channel, ClientOptions? options, char[] prefixes )
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
            
            foreach(var prefix in prefixes)
            {
                _client.AddChatCommandIdentifier(prefix);
            }

            _handler = new EventHandler(_client);
        }

        public async Task ConnectAsync()
        {
            _client.Connect();
            await Task.Delay(-1);
        }
    }
}
