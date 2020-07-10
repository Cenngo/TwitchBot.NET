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

        [Command("play", CommandType.Chat)]
        public async Task Play(string message)
        {
            await spotifyClient.QueueTrack(message);
        }

        [Command("echo", CommandType.Chat | CommandType.Whisper)]
        public void Echo(string first, string second)
        {
            CommandContext.Client.SendMessage("leMetallicat", first + " " + second, false);
        }

        [Command("auth", CommandType.Chat)]
        public async Task Auth ( )
        {
            await spotifyClient.GetAuthorization();
        }

        [Command("stop", CommandType.Chat)]
        public void Stop(string message )
        {
            CommandContext.Client.SendMessage("leMetallicat", message, false);
            Console.WriteLine("Stop");
        }

        [Command("test", CommandType.Chat)]
        public void Test()
        {
            bool test = client?.IsConnected ?? false;

            Console.WriteLine(test);
        }
    }
}
