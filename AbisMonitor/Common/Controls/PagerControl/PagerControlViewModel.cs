using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AbisMonitor.Utils;
using Common;
using GalaSoft.MvvmLight;

namespace AbisMonitor.UI.Common.Controls.PagerControl
{
    public class PagerControlViewModel : ObservableObject
    {
        public PagerControlViewModel()
        {
    
        }

        protected virtual void LoadPageData(int lineCount, int pageNum)
        {
        }

        #region pager


        private bool _fristIsEnable;
        private bool _lastIsEnable;

        private int _lineCount;
        private bool _nextIsEnable;
        private int _pageCount = 1;
        private string _pageMessage;
        private int _pageNum = 1;
        private bool _prevIsEnable;
        private int _setSelectIndex;

        private int _endNoId;
        private int _startNoId;

        private int _totalLineCount;

        private bool _isLoaded = false;


        #region PageButtonIsEnable

        public bool FristIsEnable
        {
            get { return _fristIsEnable; }
            set
            {
                _fristIsEnable = value;
                RaisePropertyChanged(() => FristIsEnable);
            }
        }

        public bool PrevIsEnable
        {
            get { return _prevIsEnable; }
            set
            {
                _prevIsEnable = value;
                RaisePropertyChanged(() => PrevIsEnable);
            }
        }

        public bool NextIsEnable
        {
            get { return _nextIsEnable; }
            set
            {
                _nextIsEnable = value;
                RaisePropertyChanged(() => NextIsEnable);
            }
        }

        public bool LastIsEnable
        {
            get { return _lastIsEnable; }
            set
            {
                _lastIsEnable = value;
                RaisePropertyChanged(() => LastIsEnable);
            }
        }

        public void SetPageButtonEnabled()
        {
            //确定分页按钮的是否可用
            if (PageCount <= 1)
            {
                FristIsEnable = false;
                PrevIsEnable = false;
                NextIsEnable = false;
                LastIsEnable = false;
            }
            else
            {
                if (PageNum == PageCount)
                {
                    FristIsEnable = true;
                    PrevIsEnable = true;
                    NextIsEnable = false;
                    LastIsEnable = false;
                }
                else if (PageNum <= 1)
                {
                    FristIsEnable = false;
                    PrevIsEnable = false;
                    NextIsEnable = true;
                    LastIsEnable = true;
                }
                else
                {
                    FristIsEnable = true;
                    PrevIsEnable = true;
                    NextIsEnable = true;
                    LastIsEnable = true;
                }
            }
        }

        #endregion

        #region PageParams

        /// <summary>
        ///     设置选择的每页显示多少行的index，从0开始。
        ///     需要在LoadPager()中初始化
        /// </summary>
        public int SetSelectIndex
        {
            get { return _setSelectIndex; }
            set
            {
                if (_setSelectIndex == value) return;
                _setSelectIndex = value;
                SetLineCountBySelectIndex(value);
                RaisePropertyChanged(() => SetSelectIndex);
            }
        }

        /// <summary>
        ///     每页显示多少行，可在LoadPager()中设置，
        ///     必须和SetSelectIndex的值指向的选择项的值相同
        /// </summary>
        public int LineCount
        {
            get { return _lineCount; }
            set
            {
                if (_lineCount == value) return;
                _lineCount = value;
                RaisePropertyChanged(() => LineCount);
            }
        }


        private int OldPageNum { get; set; }

        public int PageNum
        {
            get { return _pageNum; }
            set
            {
                if (_pageNum == value) return;
                _pageNum = value;
                RaisePropertyChanged(() => PageNum);
            }
        }

        public int TotalLineCount
        {
            get { return _totalLineCount; }
            set
            {
                _totalLineCount = value;
                if (value == 0)
                {
                    StartNoId = 0;
                    EndNoId = 0;
                    PageNum = 1;
                }
                RaisePropertyChanged(() => TotalLineCount);
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                RaisePropertyChanged(() => PageCount);
            }
        }

        public int StartNoId
        {
            get { return _startNoId; }
            set
            {
                _startNoId = value;
                RaisePropertyChanged(() => StartNoId);
            }
        }

        public int EndNoId
        {
            get { return _endNoId; }
            set
            {
                _endNoId = value;
                RaisePropertyChanged(() => EndNoId);
            }
        }

        public string PageMessage
        {
            get { return _pageMessage; }
            set
            {
                _pageMessage = value;
                RaisePropertyChanged(() => PageMessage);
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                RaisePropertyChanged(() => IsLoaded);
            }
        }

        /// <summary>
        ///     需要在实例化对象之后调用该项，初始化参数
        ///     lineCount:每夜显示多少行，分为500、1000、5000三种模式，默认为5000行。
        /// </summary>
        public void LoadPager(int lineCount = 5000)
        {
            SetPageButtonEnabled();

            LineCount = lineCount;
            switch (LineCount)
            {
                case 500:
                    SetSelectIndex = 0;
                    break;
                case 1000:
                    SetSelectIndex = 1;
                    break;
                case 5000:
                    SetSelectIndex = 2;
                    break;
                default:
                    SetSelectIndex = 2;
                    break;
            }
            this.SetPageMessage();
            OldPageNum = 1;
            IsLoaded = true;
        }

        private void SetLineCountBySelectIndex(int index)
        {
            switch (index)
            {
                case 0:
                    LineCount = 500;
                    break;
                case 1:
                    LineCount = 1000;
                    break;
                case 2:
                    LineCount = 5000;
                    break;
                default:
                    LineCount = 5000;
                    break;

            }
        }

        public void FirstPage()
        {
            PageNum = 1;
            FristIsEnable = false;
            PrevIsEnable = false;

            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        public void PrevPage()
        {
            PageNum--;

            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        public void NextPage()
        {
            PageNum++;
            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        public void LastPage()
        {
            PageNum = PageCount;

            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        public void PageNunEnter(int pageNum)
        {
            if (pageNum == OldPageNum && pageNum > 0) return;

            PageNum = pageNum;
            if (PageNum < 1)
            {
                PageNum = 1;
            }
            if (PageNum > PageCount)
            {
                PageNum = PageCount;
            }
            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        //在切换页显示行数时，默认设置为1页
        public void LineCountChanged()
        {
            PageNum = 1;

            var currentPageCount = 0;
            if (LineCount <= 0)
            {
                currentPageCount = 0;
            }
            else
            {
                currentPageCount = TotalLineCount / LineCount;
            }
            if (TotalLineCount % LineCount != 0)
            {
                currentPageCount++;
            }
            PageCount = currentPageCount;

            if (IsLoaded == false)
            {
                IsLoaded = true;
                return;
            }
            LoadPageData(LineCount, PageNum);
            OldPageNum = PageNum;
        }

        public void SetPageMessage()
        {
            if (TotalLineCount >= 0 && PageCount >= 0 && PageNum >= 1 && StartNoId >= 0 && EndNoId >= 0)
            {
                PageMessage = string.Format("共 {0:N0} 页，共 {1:N0} 条记录。当前显示 {2:N0}～{3:N0} 。", PageCount, TotalLineCount,
                    StartNoId, EndNoId);
            }
        }

        /// <summary>
        /// 设置成指定的消息提示
        /// </summary>
        /// <param name="message"></param>
        public void SetPageMessage(string message)
        {
            if (TotalLineCount >= 0 && PageCount >= 0 && PageNum >= 1 && StartNoId >= 0 && EndNoId >= 0)
            {
                PageMessage = message;
            }
        }

        #endregion

        #region ICommand

        public ICommand FirstMouseUpCmd
        {
            get { return new RelayCommand(FirstPage); }
        }

        public ICommand PrevMouseUpCmd
        {
            get { return new RelayCommand(PrevPage); }
        }

        public ICommand NextMouseUpCmd
        {
            get { return new RelayCommand(NextPage); }
        }


        public ICommand LastMouseUpCmd
        {
            get { return new RelayCommand(LastPage); }
        }

        public ICommand NumEnterCmd
        {
            get { return new RelayCommand(() => this.PageNunEnter(this.PageNum)); }
        }

        public ICommand LineCountSelectedChangedCmd
        {
            get { return new RelayCommand(LineCountChanged); }
        }

        #endregion

        #endregion
    }
}
