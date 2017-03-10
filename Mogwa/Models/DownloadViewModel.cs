using GalaSoft.MvvmLight;
using Mogwa.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
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

        /// <summary>
        /// 다운로드가 완료 되었는지 상태를 나타낸다
        /// </summary>
        private bool isDownCompleted = false;
        public bool IsDownCompleted
        {
            get { return this.isDownCompleted; }
            set
            {
                if (this.isDownCompleted != value)
                {
                    this.isDownCompleted = value;
                    this.RaisePropertyChanged("IsDownCompleted");
                }
            }
        }

        /// <summary>
        /// 현재 다운로드가 진행중인지 나타낸다
        /// </summary>
        private bool isDownProgress = false;
        public bool IsDownProgress
        {
            get { return this.isDownProgress; }
            set
            {
                if (this.isDownProgress != value)
                {
                    this.isDownProgress = value;
                    this.RaisePropertyChanged("IsDownProgress");
                }
            }
        }

        public ContentsType Type { get; set; }

        /// <summary>
        /// 비동기로 다운로드를 시작한다
        /// </summary>
        public async void BeginDownload()
        {
            try
            {
                this.Status = "다운로드 시도";
                IEnumerable<VideoInfo> videoInfos = await DownloadUrlResolver.GetDownloadUrlsAsync(this.DownloadUrl);

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
                                IEnumerable<VideoInfo> mp4Videos = videoInfos.Where(infor => infor.VideoType == VideoType.Mp4);
                                
                                foreach (var v in mp4Videos)
                                {
                                    if (v.RequiresDecryption == true)
                                    {
                                        DownloadUrlResolver.DecryptDownloadUrl(v);
                                    }

                                    var bgDownloader = new BackgroundDownloader();
                                    StorageFolder saveDir = KnownFolders.VideosLibrary;
                                    var part = await saveDir.CreateFileAsync(
                                        v.Title + v.VideoExtension, CreationCollisionOption.ReplaceExisting);
                                    var operation = bgDownloader.CreateDownload(new Uri(v.DownloadUrl), part);

                                    await this.DoDownloadAsync(operation);
                                    break;
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                this.Status = "다운로드 실패";
                var dialog = new MessageDialog(e.Message);
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// 비동기로 다운로드를 진행하며 그 와 같이 진행할 콜백도 정한다
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        private async Task DoDownloadAsync(DownloadOperation operation)
        {
            var callback = new Progress<DownloadOperation>(DownloadCallback);
            await operation.StartAsync().AsTask(callback);
        }

        /// <summary>
        /// 다운로드가 시작되면 같이 진행되는 콜백 함수로
        /// 다운로드의 상태와 다운로드가 얼마나 진행되었는지를 나타내는데 사용된다
        /// </summary>
        /// <param name="operation"></param>
        private void DownloadCallback(DownloadOperation operation)
        {
            this.Percentage = ((double)operation.Progress.BytesReceived / operation.Progress.TotalBytesToReceive) * 100;
            this.Status = $"다운로드 중 ({(int)this.Percentage}%)";
            this.IsDownProgress = true;
            if (this.Percentage >= 100)
            {
                this.Status = "다운로드 완료";
                this.IsDownCompleted = true;
                this.IsDownProgress = false;
            }
        }
    }
}
