using GalaSoft.MvvmLight;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using YoutubeExtractor;

namespace Mogwa.Utils
{
    public class YoutubeNode : ObservableObject
    {
        public string Title { get; set; } = "";
        public string DefaultThumbnail { get; set; } = "";
        public string MediumThumbnail { get; set; } = "";
        public string LargeThumbnail { get; set; } = "";
        public string VideoId { get; set; } = "";
        public string ChannelId { get; set; } = "";
        public string PublishedAt { get; set; } = "";
        private string videoUrl = "";

        /// <summary>
        /// 선택이 된 경우 true, 선택이 되지 않은 경우 false
        /// </summary>
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.RaisePropertyChanged("IsSelected");
                }
            }
        }
        public string VideoUrl
        {
            get { return this.videoUrl; }
            set { this.videoUrl = value; }
        }

    }
    class YoutubeHelper
    {
        private YouTubeService youtube = null;

        public YoutubeHelper()
        {
            if (this.youtube == null)
            {
                this.youtube = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyCNcsvp46N_jtai-pKdizTL_pKAqQGIjlk",
                    ApplicationName = "Mogwa"
                });
            }
        }

        public async Task<IEnumerable<YoutubeNode>> SearchByTitle(string title, int limit=50)
        {
            try
            {
                var req = this.youtube.Search.List("snippet");
                req.Q = title;
                req.MaxResults = 50;

                var result = await req.ExecuteAsync();
                var nodes = new List<YoutubeNode>();
                foreach (var resultItem in result.Items)
                {
                    nodes.Add(new YoutubeNode()
                    {
                        VideoId = resultItem.Id.VideoId,
                        ChannelId = resultItem.Snippet.ChannelId,
                        PublishedAt = resultItem.Snippet.PublishedAt.ToString(),
                        Title = resultItem.Snippet.Title,
                        DefaultThumbnail = resultItem.Snippet.Thumbnails.Default__.Url,
                        MediumThumbnail = resultItem.Snippet.Thumbnails.Medium.Url,
                        LargeThumbnail = resultItem.Snippet.Thumbnails.High.Url,
                        VideoUrl = "https://www.youtube.com/v/" + resultItem.Id.VideoId
                    });
                }
                return nodes;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
