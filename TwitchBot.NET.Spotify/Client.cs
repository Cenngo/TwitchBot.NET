using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private AuthorizationCodeResponse _code;

        private AuthorizationCodeTokenResponse _currToken;

        public Client(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task GetAuthorization()
        {
            _server = new EmbedIOAuthServer(new Uri("https://localhost:5000/callback"), 5000);
            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;

            await _server.Start();

            var request = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserModifyPlaybackState }
            };
            BrowserUtil.Open(request.ToUri());
        }

        private async Task OnAuthorizationCodeReceived ( object sender, AuthorizationCodeResponse response )
        {
            Console.WriteLine("Stopped Server");
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                _clientId, _clientSecret, response.Code, new Uri("https://localhost:5000/callback")
              )
            );

            _client = new SpotifyClient(tokenResponse.AccessToken);
            Console.WriteLine("Created Client");
        }

        public async Task RefreshToken()
        {

        }

        public async Task<FullTrack> QueueTrack(string query)
        {
            var searchResponse = await _client.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
            var track = searchResponse.Tracks.Items[0];

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
