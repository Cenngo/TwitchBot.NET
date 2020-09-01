using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using Swan.Parsers;

namespace TwitchBot.NET.Spotify
{
    public class Client
    {
        private SpotifyClient _client;
        private EmbedIOAuthServer _server;

        private readonly string _clientId;
        private readonly string _clientSecret;

        private EventWaitHandle WaitHandle;

        private AuthorizationCodeTokenResponse _currToken;

        public Client(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;

            WaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public async Task GetAuthorization()
        {
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);

            await _server.Start();
            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;

            var request = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> { Scopes.UserModifyPlaybackState, Scopes.UserReadPlaybackState, Scopes.UserReadCurrentlyPlaying}
            };
            BrowserUtil.Open(request.ToUri());

            if(!WaitHandle.WaitOne(TimeSpan.FromMinutes(2)))
            {
                Console.Clear();
                Console.WriteLine("Timed out");
                Environment.Exit(1);
            }
        }

        private async Task OnAuthorizationCodeReceived ( object sender, AuthorizationCodeResponse response )
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                _clientId, _clientSecret, response.Code, new Uri("http://localhost:5000/callback")
              )
            );

            _currToken = tokenResponse;

            _client = new SpotifyClient(tokenResponse.AccessToken);

            WaitHandle.Set();
            Console.Clear();
        }

        public async Task<FullTrack> QueueTrack(string query)
        {
            Console.WriteLine($"Searching Track: {query.ToUpper()}");
            var searchResponse = await _client.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
            var track = searchResponse.Tracks.Items[0];

            if(_client.Player == null)
            {
                throw new Exception("No Active Spotify Player Detected. Please Start Playback on a Supported Device");
                return;
            }

            if (_client.Player.GetCurrentPlayback().Result.IsPlaying)
                await _client.Player.AddToQueue(new PlayerAddToQueueRequest(track.Uri));
            else
                await _client.Player.ResumePlayback(new PlayerResumePlaybackRequest()
                { 
                    ContextUri = track.Uri
                });

            return track;
        }
    }
}
