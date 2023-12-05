using SpotifyAPI.Web;

namespace SpotifyPlaylist_alpha
{
    internal class Program
    {
        static async Task Main()
        {
            YoutubeApiCalls Youtube = new YoutubeApiCalls();
            List<string> spotifyPlaylist = await SpotifyApiCalls.GetPLayListAsync();
            YoutubeApiCalls.SetPlaylist(spotifyPlaylist);
            
            //try
            //{
            // Step 1: Get Spotify Playlist
            //List<string> spotifyPlaylist = await SpotifyApiCalls.GetPLayListAsync();

            //if (spotifyPlaylist.Count == 0)
            //{
            //    Console.WriteLine("No songs found in the Spotify playlist. Exiting.");
            //    return;
            //}

            // Step 2: Convert Spotify Playlist to YouTube Playlist
            // Assuming the first song in the Spotify playlist as the keyword for YouTube search
            //    string youtubeKeyword = spotifyPlaylist[0];
            //    var youtubeSearchRequest = YoutubeApiCalls.Searcher(youtubeKeyword);
            //    var youtubeSearchResponse = youtubeSearchRequest.Execute();

            //    YoutubeApiCalls.PlayListMaker();  // Creates a new playlist on YouTube

            //    // Convert Spotify songs to YouTube videos and add them to the playlist
            //    YoutubeApiCalls.VideoIds(spotifyPlaylist);

            //    Console.WriteLine("Conversion completed successfully!");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}
            //}



        }
    }
}


    






