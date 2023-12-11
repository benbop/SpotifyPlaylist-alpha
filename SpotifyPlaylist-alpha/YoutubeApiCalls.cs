using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Logging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Requests;

namespace SpotifyPlaylist_alpha
{


    internal class YoutubeApiCalls
    {
        private static string APIKEY = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
        private static string CLIENT_SECRET_PATH = Environment.GetEnvironmentVariable("YOUTUBE_CLIENT_SECRET_PATH");

        public static YouTubeService youTubeService;


        static YoutubeApiCalls()
        {
            var credential = GetCredential().Result; // Using .Result to wait for the task to complete
            youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "PlayListMaker"
            });
        }

        private static async Task<UserCredential> GetCredential()
        {
            using (var stream = new FileStream(CLIENT_SECRET_PATH, FileMode.Open, FileAccess.Read))
            {
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("PlayListMaker"));
            }
        }

        public static SearchResource.ListRequest Searcher(string keyword)
        {
            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = keyword;
            searchListRequest.MaxResults = 1;
            return searchListRequest;
        }

        public static Playlist PlayListMaker()
        {
            Console.WriteLine("Starting PlayListMaker...");

            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = "YourPlaylistTitle"; // Set your desired playlist title
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";

            var playlistsInsertRequest = youTubeService.Playlists.Insert(newPlaylist, "snippet,status");
            var playlistResponse = playlistsInsertRequest.Execute();

            Console.WriteLine("Playlist created.");

            return playlistResponse;
        }
        public static async Task AddToPlaylist(string playlistId, string videoid)
        {
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = playlistId;
            newPlaylistItem.Snippet.ResourceId = new ResourceId { Kind = "youtube#video", VideoId = videoid };

            try
            {
                var playlistItemsInsertRequest = youTubeService.PlaylistItems.Insert(newPlaylistItem, "snippet");
                var playlistItemResponse = await playlistItemsInsertRequest.ExecuteAsync();
                Console.WriteLine($"Added video with ID: {videoid} to the playlist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding video with ID {videoid}: {ex.Message}");
                // Log additional details if needed
            }

        }


        public static async Task SetPlaylist(List<string> PlayList)
        {
            var playlistResponse = PlayListMaker();





            foreach (var song in PlayList)
            {


                Console.WriteLine($"Processing song: {song}");

                

                try
                {
                    var searchRequest = YoutubeApiCalls.Searcher(song);
                    var searchResponse = searchRequest.Execute();

                    if (searchResponse.Items.Count > 0)
                    {
                        var firstResult = searchResponse.Items[0];
                         await AddToPlaylist(playlistResponse.Id, firstResult.Id.VideoId);
                        Console.WriteLine($"The song {song} has been added");
                    }
                }
                catch (Google.GoogleApiException ex)
                {
                    Console.WriteLine($"Google API Exception: {ex.Message}");
                    Console.WriteLine($"HTTP Status Code: {ex.HttpStatusCode}");
                    // Add more details as needed

                    // Continue to the next song on exception
                    Console.WriteLine($"Skipping song: {song}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Continue to the next song on exception
                    Console.WriteLine($"Skipping song: {song}");
                }

            }


            Console.WriteLine("SetPlaylist completed.");
        }
    }
        }

    
    

