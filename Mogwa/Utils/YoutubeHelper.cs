using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogwa.Utils
{
    class YoutubeNode
    {
        public string Title { get; set; } = "";
        public string DefaultThumbnail { get; set; } = "";
        public string MediumThumbnail { get; set; } = "";
        public string LargeThumbnail { get; set; } = "";
        public string VideoId { get; set; } = "";
        public string ChannelId { get; set; } = "";
        public string PublishedAt { get; set; } = "";

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

        public async Task<IEnumerable<YoutubeNode>> SearchByTitle(string title, int limit=30)
        {
            try
            {
                var req = this.youtube.Search.List("snippet");
                req.Q = title;
                req.MaxResults = limit;

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
                        LargeThumbnail = resultItem.Snippet.Thumbnails.High.Url
                    });
                }
                return nodes;
            }
            catch
            {
                return null;
            }
        }
    }
}
