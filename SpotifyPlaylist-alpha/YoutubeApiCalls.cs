using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyPlaylist_alpha
{


    internal class YoutubeApiCalls
    {
        private static string APIKEY = "AIzaSyCVR0y4mXry3R82nq-ez4ylaIFG3X--GSw";

        private static string CLIENT_SECRET_PATH = @"C:\Users\bener\Downloads\client_secret_580617838845-hk8ue2u06mqvefjuemee1su917bu116c.apps.googleusercontent.com.json";

        public static YouTubeService youTubeService;

        static YoutubeApiCalls()
        {
            var credential = GetCredential();
            youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "PlayListMaker"
            });
        }

        private static UserCredential GetCredential()
        {
            using (var stream = new FileStream(CLIENT_SECRET_PATH, FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("PlayListMaker")).Result;
            }
        }

        private static Playlist newPlaylist = new Playlist();
        static public SearchResource.ListRequest Searcher(string keyword)
        {
            

            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = keyword;
            return searchListRequest;


        }
        public static void PlayListMaker()// playlist creator 
        {
            
            int i = 0;
            
            newPlaylist.Snippet = new PlaylistSnippet();
            
                newPlaylist.Snippet.Title = $"PlayListFromSpotify-{i}";
            
            
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";
            var playlistsInsertRequest = youTubeService.Playlists.Insert(newPlaylist, "snippet,status");
            var playlistResponse = playlistsInsertRequest.Execute();
        }
        public static void VideoIds(List<string> PlayList) // getsvideo ids
        {
            List<string> ids = new List<string>();

            foreach (var song in PlayList)
            {
                var searchRequest = YoutubeApiCalls.Searcher(song);
                
                    var searchResponse = searchRequest.Execute();

                  
                       
                        var firstResult = searchResponse.Items[0];
                ids.Add(firstResult.Id.ToString());

            }

            var playlistsInsertRequest = youTubeService.Playlists.Insert(newPlaylist, "snippet,status");
            
            var playlistResponse = playlistsInsertRequest.Execute();

            foreach (var videoId in ids)
            {
                var newPlaylistItem = new PlaylistItem();
                newPlaylistItem.Snippet = new PlaylistItemSnippet();
                newPlaylistItem.Snippet.PlaylistId = playlistResponse.Id;
                newPlaylistItem.Snippet.ResourceId = new ResourceId { Kind = "youtube#video", VideoId = videoId };

                
                var playlistItemsInsertRequest = youTubeService.PlaylistItems.Insert(newPlaylistItem, "snippet");
                var playlistItemResponse = playlistItemsInsertRequest.Execute();

                
            }
        }
        

        }
    }

