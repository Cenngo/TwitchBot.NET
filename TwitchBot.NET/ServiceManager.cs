using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace TwitchBot.NET
{
    public class ServiceManager
    {
        private TwitchClient client;
        private TwitchBot.NET.Spotify.Client SpotifyClient;
        public ServiceManager(TwitchClient client)
        {
            this.client = client;
            SpotifyClient = new Spotify.Client("8bafdf8d107643c98fabbe737ca54664", "c7949d21ec794528b5fdfac753034fd8");
        }

        public IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection().AddSingleton(client).AddSingleton(SpotifyClient).BuildServiceProvider();
        }
    }
}
