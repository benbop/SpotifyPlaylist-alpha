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

           var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");

            var request = new ClientCredentialsRequest(clientId, clientSecret);
            var response = await new OAuthClient(config).RequestToken(request);

            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            return spotify;
        }


        public static async Task<List<string>> GetPLayListAsync() // Return a list that contains the names of the artists and the song
        {
            List<string> PlayListSongs = new List<string>();
            SpotifyApiCalls SpotifyApi = new SpotifyApiCalls();


            var playlist = await SpotifyApiCalls.GetPlaylistAsync("YOUR_PLAYLIST_ID");
            foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
            {
                if (item.Track is FullTrack track)
                {
                    var Artists = track.Artists;
                    var artistNames = string.Join(", ", Artists.Select(a => a.Name));
                    PlayListSongs.Add($"{track.Name} By {artistNames}");

                }
            }
            return PlayListSongs;
        }

    }
}
