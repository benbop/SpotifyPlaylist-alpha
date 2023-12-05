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
        private static string APIKEY = "AIzaSyCVR0y4mXry3R82nq-ez4ylaIFG3X--GSw";

        private static string CLIENT_SECRET_PATH = @"C:\Users\bener\Downloads\client_secret_580617838845-hk8ue2u06mqvefjuemee1su917bu116c.apps.googleusercontent.com.json";

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
        public static void AddToPlaylist(string playlistId, string videoId)
        {
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = playlistId;
            newPlaylistItem.Snippet.ResourceId = new ResourceId { Kind = "youtube#video", VideoId = videoId };

            var playlistItemsInsertRequest = youTubeService.PlaylistItems.Insert(newPlaylistItem, "snippet");
            var playlistItemResponse = playlistItemsInsertRequest.Execute();
        }


        public static void SetPlaylist(List<string> PlayList, int maxApiCalls = 100)
        {
            var playlistResponse = PlayListMaker();


           

            
            foreach (var song in PlayList)
            {
                if (apiCallsCounter >= maxApiCalls)
                {
                    Console.WriteLine($"Reached the maximum number of API calls ({maxApiCalls}). Exiting loop.");
                    break;
                }

                Console.WriteLine($"Processing song: {song}");



                try
                {
                    var searchRequest = YoutubeApiCalls.Searcher(song);
                    var searchResponse = searchRequest.Execute();

                    if (searchResponse.Items.Count > 0)
                    {
                        var firstResult = searchResponse.Items[0];
                        AddToPlaylist(playlistResponse.Id, firstResult.Id.VideoId);
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

            Console.WriteLine($"Total API Calls: {apiCallsCounter}");
            Console.WriteLine("SetPlaylist completed.");
        }
    }
        }

    
    

