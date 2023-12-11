using SpotifyAPI.Web;

namespace SpotifyPlaylist_alpha
{
    internal class Program
    {
        static async Task Main()
        {
            YoutubeApiCalls Youtube = new YoutubeApiCalls();
            List<string> spotifyPlaylist = await SpotifyApiCalls.GetPLayListAsync();
            await YoutubeApiCalls.SetPlaylist(spotifyPlaylist);
        }
    }
}


    






