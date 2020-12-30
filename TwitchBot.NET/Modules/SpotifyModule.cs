using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.NET.Commands;
using TwitchBot.NET.Commands.Attributes;
using TwitchBot.NET.Commands.Enums;
using TwitchBot.NET.Commands.Models;
using TwitchBot.NET.Spotify;
using TwitchLib.Client;

namespace TwitchBot.NET.Modules
{
    public class SpotifyModule : CommandModule
    {
        TwitchClient client;
        Client spotifyClient;
        public SpotifyModule(TwitchClient client, Client spotifyClient)
        {
            this.client = client;
            this.spotifyClient = spotifyClient;
        }

        [Command("ping", CommandType.Chat)]
        public async Task Ping()
        {
            Reply("Pong", false);
        }

        [Command("play", CommandType.Chat)]
        public async Task Play(params string[] message)
        {
            var query = String.Join(' ', message);
            Reply( $"Searching: {query}");
            await spotifyClient.QueueTrack(query);
        }

        [Command("echo", CommandType.Chat)]
        public async Task Echo(string first, string second)
        {
            Reply($"First: {first}, Second: {second}");
        }

        [Command("auth", CommandType.Chat)]
        public async Task Auth ( )
        {
            await spotifyClient.GetAuthorization();
        }

        [Command("test", CommandType.Chat)]
        public void Test()
        {
            bool test = client?.IsConnected ?? false;

            Console.WriteLine(test);
        }
    }
}
