using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TwitchLib.Client;

namespace TwitchBot.NET
{
    public class ServiceManager
    {
        private TwitchClient client;
        private Spotify.Client SpotifyClient;
        public ServiceManager(TwitchClient client)
        {
            this.client = client;
            SpotifyClient = new Spotify.Client("8bafdf8d107643c98fabbe737ca54664", "c7949d21ec794528b5fdfac753034fd8");
            SpotifyClient.GetAuthorization().GetAwaiter().GetResult();
        }

        public IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(SpotifyClient)
                .BuildServiceProvider();
        }
    }
}
