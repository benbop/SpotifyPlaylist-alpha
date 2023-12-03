using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyPlaylist_alpha
{
    
    internal class SpotifyApiCalls
    {
       
        
        public static async Task<FullPlaylist> GetPlaylistAsync(string playlistId)
        {
           
            FullPlaylist playlist = await getAccsesTokenAsync().Result.Playlists.Get(playlistId);
            return playlist;
        }
        public static async Task<SpotifyClient> getAccsesTokenAsync()
        {
            var config = SpotifyClientConfig.CreateDefault();

            var request = new ClientCredentialsRequest("503a274f9a5c492394417f1b73716f5a", "6707f44d2ece492596137c920f2e24ec");
            var response = await new OAuthClient(config).RequestToken(request);

            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            return spotify;
        }

    }
}
