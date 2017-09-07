using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AbisMonitor.UI.Common.Controls.PagerControl
{
    /// <summary>
    /// PagerControl.xaml 的交互逻辑
    /// </summary>
    public delegate void PagerButtonClickHandler();

    public delegate void PagerButtonPageNumChangeHandler(int pageNum);
    /// <summary>
    /// PagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PagerControl : UserControl, INotifyPropertyChanged
    {
        public PagerControl()
        {
            InitializeComponent();
        }

        private void TxtPageNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                || (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Enter)
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        #region Properties

        public static readonly DependencyProperty LineCountProperty =
            DependencyProperty.Register("LineCount", typeof(int), typeof(PagerControl), new PropertyMetadata());

        public static readonly DependencyProperty PageNumProperty = DependencyProperty.Register("PageNum",
            typeof(int), typeof(PagerControl), new PropertyMetadata(1));


        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount",
            typeof(int), typeof(PagerControl), new PropertyMetadata(1));

        public static readonly DependencyProperty TotalLineCountProperty = DependencyProperty.Register("TotalLineCount",
            typeof(int), typeof(PagerControl), new PropertyMetadata(0));


        public static readonly DependencyProperty StartNoIdProperty = DependencyProperty.Register("StartNoId",
            typeof(int), typeof(PagerControl), new PropertyMetadata(0));

        public static readonly DependencyProperty EndNoIdProperty = DependencyProperty.Register("EndNoId",
            typeof(int), typeof(PagerControl), new PropertyMetadata(0));

        public static readonly DependencyProperty PageMessageProperty = DependencyProperty.Register("PageMessage",
            typeof(string), typeof(PagerControl), new PropertyMetadata());

        public static readonly DependencyProperty SetSelectedIndexProperty =
            DependencyProperty.Register("SetSelectIndex", typeof(int), typeof(PagerControl), new PropertyMetadata(2));

        public static readonly DependencyProperty FirstMouseUpCommandProperty = DependencyProperty.Register(
            "FirstMouseUpCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty PrevMouseUpCommandProperty = DependencyProperty.Register(
            "PrevMouseUpCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty NextMouseUpCommandProperty = DependencyProperty.Register(
            "NextMouseUpCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty LastMouseUpCommandProperty = DependencyProperty.Register(
            "LastMouseUpCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty NumEnterCommandProperty = DependencyProperty.Register(
            "NumEnterCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty LineCountSelectedChangedCommandProperty = DependencyProperty.Register(
            "LineCountSelectedChangedCommand", typeof(ICommand), typeof(PagerControl), new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty FristIsEnabledProperty = DependencyProperty.Register(
            "FristIsEnabled", typeof(bool), typeof(PagerControl), new PropertyMetadata(false));

        public static readonly DependencyProperty PrevIsEnabledProperty = DependencyProperty.Register(
            "PrevIsEnabled", typeof(bool), typeof(PagerControl), new PropertyMetadata(false));

        public static readonly DependencyProperty NextIsEnabledProperty = DependencyProperty.Register(
            "NextIsEnabled", typeof(bool), typeof(PagerControl), new PropertyMetadata(false));

        public static readonly DependencyProperty LastIsEnabledProperty = DependencyProperty.Register(
            "LastIsEnabled", typeof(bool), typeof(PagerControl), new PropertyMetadata(false));



        public bool FristIsEnabled
        {
            get { return (bool)GetValue(FristIsEnabledProperty); }
            set
            {
                SetValue(FristIsEnabledProperty, value);
                NotifyPropertyChange("FristIsEnabled");
            }
        }

        public bool PrevIsEnabled
        {
            get { return (bool)GetValue(PrevIsEnabledProperty); }
            set
            {
                SetValue(PrevIsEnabledProperty, value);
                NotifyPropertyChange("PrevIsEnabled");
            }
        }

        public bool NextIsEnabled
        {
            get { return (bool)GetValue(NextIsEnabledProperty); }
            set
            {
                SetValue(NextIsEnabledProperty, value);
                NotifyPropertyChange("NextIsEnabled");
            }
        }

        public bool LastIsEnabled
        {
            get { return (bool)GetValue(LastIsEnabledProperty); }
            set
            {
                SetValue(LastIsEnabledProperty, value);
                NotifyPropertyChange("LastIsEnabled");
            }
        }

        /// <summary>
        ///     每页显示行数
        /// </summary>
        public int LineCount
        {
            get { return (int)GetValue(LineCountProperty); }
            set
            {
                SetValue(LineCountProperty, value);
                NotifyPropertyChange("LineCount");
            }
        }

        /// <summary>
        ///     当前页数
        /// </summary>
        public int PageNum
        {
            get { return (int)GetValue(PageNumProperty); }
            set
            {
                SetValue(PageNumProperty, value);
                NotifyPropertyChange("PageNum");
            }
        }

        /// <summary>
        ///     总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set
            {
                SetValue(PageCountProperty, value);
                NotifyPropertyChange("PageCount");
            }
        }

        /// <summary>
        ///     总行数
        /// </summary>
        public int TotalLineCount
        {
            get { return (int)GetValue(TotalLineCountProperty); }
            set
            {
                SetValue(TotalLineCountProperty, value);
                NotifyPropertyChange("TotalLineCount");
            }
        }

        /// <summary>
        ///     当前页起始序号
        /// </summary>
        public int StartNoId
        {
            get { return (int)GetValue(StartNoIdProperty); }
            set
            {
                SetValue(StartNoIdProperty, value);
                NotifyPropertyChange("StartNoId");
            }
        }

        /// <summary>
        ///     当前页结束序号
        /// </summary>
        public int EndNoId
        {
            get { return (int)GetValue(EndNoIdProperty); }
            set
            {
                SetValue(EndNoIdProperty, value);
                NotifyPropertyChange("EndNoId");
            }
        }


        public string PageMessage
        {
            get { return (string)GetValue(PageMessageProperty); }
            set
            {
                SetValue(PageMessageProperty, value);
                NotifyPropertyChange("PageMessage");
            }
        }

        public int SetSelectIndex
        {
            get { return (int)GetValue(SetSelectedIndexProperty); }
            set
            {
                SetValue(SetSelectedIndexProperty, value);
                NotifyPropertyChange("SetSelectIndex");
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region event

        //private void TxtPageNum_OnPreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        if (PageNumEnterEvent != null)
        //        {
        //            int tempPageNum = GetPageNum();
        //            PageNumEnterEvent(GetPageNum());

        //            if (NumEnterCommand != null)
        //            {
        //                NumEnterCommand.Execute(tempPageNum);
        //            }
        //        }
        //    }
        //}



        private int GetPageNum()
        {
            int pageNum = 1;

            int.TryParse(txtPageNum.Text, out pageNum);

            return pageNum;
        }

        #endregion

        #region ICommand

        public ICommand FirstMouseUpCommand
        {
            get { return (ICommand)GetValue(FirstMouseUpCommandProperty); }
            set { SetValue(FirstMouseUpCommandProperty, value); }
        }

        public ICommand PrevMouseUpCommand
        {
            get { return (ICommand)GetValue(PrevMouseUpCommandProperty); }
            set { SetValue(PrevMouseUpCommandProperty, value); }
        }

        public ICommand NextMouseUpCommand
        {
            get { return (ICommand)GetValue(NextMouseUpCommandProperty); }
            set { SetValue(NextMouseUpCommandProperty, value); }
        }

        public ICommand LastMouseUpCommand
        {
            get { return (ICommand)GetValue(LastMouseUpCommandProperty); }
            set { SetValue(LastMouseUpCommandProperty, value); }
        }

        public ICommand NumEnterCommand
        {
            get { return (ICommand)GetValue(NumEnterCommandProperty); }
            set { SetValue(NumEnterCommandProperty, value); }
        }


        public ICommand LineCountSelectedChangedCommand
        {
            get { return (ICommand)GetValue(LineCountSelectedChangedCommandProperty); }
            set { SetValue(LineCountSelectedChangedCommandProperty, value); }
        }

        #endregion
    }
}
