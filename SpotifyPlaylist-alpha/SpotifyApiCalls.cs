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


        public static async Task<List<string>> GetPLayListAsync() // Return a list that contains the names of the artists and the song
        {
            List<string> PlayListSongs = new List<string>();
            SpotifyApiCalls SpotifyApi = new SpotifyApiCalls();


            var playlist = await SpotifyApiCalls.GetPlaylistAsync("6lyWdNcyXZmZkI7W46Dd7M");
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
