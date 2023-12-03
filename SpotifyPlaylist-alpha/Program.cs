using SpotifyAPI.Web;

namespace SpotifyPlaylist_alpha
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> playlistSongs = GetPLayListAsync().Result;
            
        }
         public static async Task<List<string>> GetPLayListAsync()
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