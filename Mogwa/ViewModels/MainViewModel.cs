using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        #endregion
    }
}
