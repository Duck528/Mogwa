using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mogwa.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Mogwa.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        #region NavMenu
        /// <summary>
        /// 네비게이트 메뉴의 펼친 상태를 알려준다
        /// false: 접힌 상태
        /// true: 펼친 상태
        /// </summary>
        private bool isNavMenuOpened = false;
        public bool IsNavMenuOpened
        {
            get { return this.isNavMenuOpened; }
            set
            {
                if (this.isNavMenuOpened != value)
                {
                    this.isNavMenuOpened = value;
                    this.RaisePropertyChanged("IsNavMenuOpened");
                }
            }
        }

        /// <summary>
        /// 네비게이트 메뉴를 토글링한다
        /// 펼친 상태 -> 접힌 상태
        /// 접힌 상태 -> 펼친 상태
        /// </summary>
        private ICommand toggleNavMenu = null;
        public ICommand ToggleNavMenu
        {
            get
            {
                if (this.toggleNavMenu == null)
                {
                    this.toggleNavMenu = new RelayCommand(() =>
                    {
                        this.IsNavMenuOpened = !this.IsNavMenuOpened;
                    });
                }
                return this.toggleNavMenu;
            }
        }
        #endregion

        #region TopStatus
        /// <summary>
        /// 뒤로가기 버튼 활성화 여부를 나타낸다
        /// </summary>
        private bool enableBack = false;
        public bool EnableBack
        {
            get { return this.enableBack; }
            set
            {
                if (this.enableBack != value)
                {
                    this.enableBack = value;
                    this.RaisePropertyChanged("EnableBack");
                }
            }
        }

        /// <summary>
        /// 검색 메뉴의 펼친 상태를 알려준다
        /// true: 펼친 상태
        /// false: 접힌 상태
        /// </summary>
        private bool isSearchOpened = false;
        public bool IsSearchOpened
        {
            get { return this.isSearchOpened; }
            set
            {
                if (this.isSearchOpened != value)
                {
                    this.isSearchOpened = value;
                    this.RaisePropertyChanged("IsSearchOpened");
                }
            }
        }

        /// <summary>
        /// 검색 메뉴를 토글링한다
        /// </summary>
        private ICommand toggleSearch = null;
        public ICommand ToggleSearch
        {
            get
            {
                if (this.toggleSearch == null)
                {
                    this.toggleSearch = new RelayCommand(() =>
                    {
                        this.IsSearchOpened = !this.IsSearchOpened;
                    });
                }
                return this.toggleSearch;
            }
        }

        /// <summary>
        /// 검색 키워드
        /// </summary>
        private string searchKeyWord = "";
        public string SearchKeyWord
        {
            get { return this.searchKeyWord; }
            set
            {
                if (this.searchKeyWord.Equals(value) == false)
                {
                    this.searchKeyWord = value;
                    this.RaisePropertyChanged("SearchKeyWord");
                }
            }
        }

        /// <summary>
        /// 유튜브를 통해 검색 작업을 수행한다
        /// </summary>
        private ICommand doYoutubeSearch = null;
        public ICommand DoYoutubeSearch
        {
            get
            {
                if (this.doYoutubeSearch == null)
                {
                    this.doYoutubeSearch = new RelayCommand(async () =>
                    {
                        var youtubeHelper = new YoutubeHelper();
                        var results = await youtubeHelper.SearchByTitle(this.SearchKeyWord);
                        this.YoutubeSearchResults = new ObservableCollection<YoutubeNode>(results);
                        this.IsSearchResultVisible = true;
                    });
                }
                return this.doYoutubeSearch;
            }
        }

        /// <summary>
        /// 유튜브 검색 결과 데이터
        /// </summary>
        private ObservableCollection<YoutubeNode> youtubeSearchResults = null;
        public ObservableCollection<YoutubeNode> YoutubeSearchResults
        {
            get { return this.youtubeSearchResults; }
            set
            {
                if (this.youtubeSearchResults != value)
                {
                    this.youtubeSearchResults = value;
                    this.NumYoutubeSearchResults = this.YoutubeSearchResults.Count();
                    this.RaisePropertyChanged("YoutubeSearchResults");
                }
            }
        }

        /// <summary>
        /// 유튜브 검색 결과 데이터의 수
        /// </summary>
        private int numYoutubeSearchResults = 0;
        public int NumYoutubeSearchResults
        {
            get { return this.numYoutubeSearchResults; }
            set
            {
                if (this.numYoutubeSearchResults != value)
                {
                    this.numYoutubeSearchResults = value;
                    this.RaisePropertyChanged("NumYoutubeSearchResults");
                }
            }
        }
        #endregion

        #region ContentsVisibilities
        private bool isSearchResultVisible = false;
        public bool IsSearchResultVisible
        {
            get { return this.isSearchOpened; }
            set
            {
                if (this.isSearchResultVisible != value)
                {
                    this.isSearchResultVisible = value;
                    this.RaisePropertyChanged("IsSearchResultVisible");
                }
            }
        }
        #endregion

        #region Contents
        private ICommand doOpenFlyoutMenu = null;
        public ICommand DoOpenFlyoutMenu
        {
            get
            {
                if (this.doOpenFlyoutMenu == null)
                {
                    this.doOpenFlyoutMenu = new RelayCommand<object>((sender) =>
                    {
                        FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                    });
                }
                return this.doOpenFlyoutMenu;
            }
        }
        #endregion
    }
}
