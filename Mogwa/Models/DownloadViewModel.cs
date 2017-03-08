using GalaSoft.MvvmLight;
using Mogwa.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using YoutubeExtractor;

namespace Mogwa.Models
{
    public enum ContentsType { MP4, Audio };
    public class DownloadViewModel : ObservableObject
    {
        private string downloadUrl = "";
        public string DownloadUrl
        {
            get { return this.downloadUrl; }
            set
            {
                if (this.downloadUrl.Equals(value) == false)
                {
                    this.downloadUrl = value;
                    this.RaisePropertyChanged("DownloadUrl");
                }
            }
        }
        private string title = "";
        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title.Equals(value) == false)
                {
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        private string thumbnailUrl = "";
        public string ThumbnailUrl
        {
            get { return this.thumbnailUrl; }
            set
            {
                if (this.thumbnailUrl.Equals(value) == false)
                {
                    this.thumbnailUrl = value;
                    this.RaisePropertyChanged("ThumbnailUrl");
                }
            }
        }

        private double percentage = 0;
        public double Percentage
        {
            get { return this.percentage; }
            set
            {
                if (this.percentage != value)
                {
                    this.percentage = value;
                    this.RaisePropertyChanged("Percentage");
                }
            }
        }

        private string status = "";
        public string Status
        {
            get { return this.status; }
            set
            {
                if (this.status.Equals(value) == false)
                {
                    this.status = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }

        private string totalBytes = "";
        public string TotalBytes
        {
            get { return this.totalBytes; }
            set
            {
                if (this.totalBytes.Equals(value) == false)
                {
                    this.totalBytes = value;
                    this.RaisePropertyChanged("TotalBytes");
                }
            }
        }

        public ContentsType Type { get; set; }

        public async void BeginDownload()
        {
            IEnumerable<VideoInfo> videoInfos = 
                DownloadUrlResolver.GetDownloadUrls(this.DownloadUrl);
            if (videoInfos != null)
            {
                switch (this.Type)
                {
                    case ContentsType.Audio:
                        {
                            break;
                        }
                    case ContentsType.MP4:
                        {
                            VideoInfo videoInfo = videoInfos.First(infor => infor.VideoType == VideoType.Mp4);
                            if (videoInfo.RequiresDecryption == true)
                            {
                                DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                            }

                            var bgDownloader = new BackgroundDownloader();
                            StorageFolder saveDir = KnownFolders.MusicLibrary;
                            var part = await saveDir.CreateFileAsync(
                                videoInfo.Title + videoInfo.VideoExtension, CreationCollisionOption.ReplaceExisting);
                            var operation = bgDownloader.CreateDownload(new Uri(videoInfo.DownloadUrl), part);

                            await this.DoDownloadAsync(operation);
                            break;
                        }
                }
            }
        }

        private async Task DoDownloadAsync(DownloadOperation operation)
        {
            var callback = new Progress<DownloadOperation>(DownloadCallback);
            await operation.StartAsync().AsTask(callback);
        }

        private void DownloadCallback(DownloadOperation operation)
        {
            this.Status = "다운로드 중";
            this.Percentage = ((double)operation.Progress.BytesReceived / operation.Progress.TotalBytesToReceive) * 100;
            if (this.Percentage >= 100)
            {
                this.Status = "다운로드 완료";
            }
        }
    }
}
