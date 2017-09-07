using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AbisMonitor.UI.Common.Controls
{
    /// <summary>
    /// TimeSelectControl.xaml 的交互逻辑
    /// </summary>
    /// TimeSelectControl.xaml 的交互逻辑
    /// </summary>
    public partial class TimeSelectControl : UserControl, INotifyPropertyChanged
    {
        public TimeSelectControl()
        {
            InitializeComponent();
            Init();
            this.DataContext = this;
        }

        private void Init()
        {
            TimeSelectedList = new ObservableCollection<TimeSelectedItem>()
            {
                new TimeSelectedItem() {Key = 0, Vaule = Application.Current.FindResource("Today") as string},
                new TimeSelectedItem() {Key = 1, Vaule = Application.Current.FindResource("Yesterday") as string},
                new TimeSelectedItem() {Key = 2, Vaule = Application.Current.FindResource("BeforeYesterday") as string},
                new TimeSelectedItem() {Key = 3, Vaule = Application.Current.FindResource("ThisWeek") as string},
                new TimeSelectedItem() {Key = 4, Vaule = Application.Current.FindResource("LastWeek") as string},
                new TimeSelectedItem() {Key = 5, Vaule = Application.Current.FindResource("ThisMonth") as string},
                new TimeSelectedItem() {Key = 6, Vaule = Application.Current.FindResource("LastMonth") as string},
                new TimeSelectedItem() {Key = 7, Vaule = Application.Current.FindResource("TheDayBefore") as string},
                new TimeSelectedItem() {Key = 8, Vaule = Application.Current.FindResource("TheDayAfter") as string},
            };
        }

        private void BtnTime_OnClick(object sender, RoutedEventArgs e)
        {
            this.TimeSelectedItemKey = null;
            this.popupTime.IsOpen = true;
        }

        public static readonly DependencyProperty TimeSelectedCommandProperty = DependencyProperty.Register(
            "TimeSelectedCommand", typeof(ICommand), typeof(TimeSelectControl), new PropertyMetadata(default(ICommand)));

        public ICommand TimeSelectedCommand
        {
            get { return (ICommand)GetValue(TimeSelectedCommandProperty); }
            set { SetValue(TimeSelectedCommandProperty, value); }
        }


        private int? _timeSelectedItemKey;

        public int? TimeSelectedItemKey
        {
            get { return _timeSelectedItemKey; }
            set
            {
                _timeSelectedItemKey = value;
                this.popupTime.IsOpen = false;

                if (_timeSelectedItemKey != null)
                {
                    if (TimeSelectedCommand != null)
                    {
                        TimeSelectedCommand.Execute(_timeSelectedItemKey);
                    }
                }
                this.OnPropertyChanged("TimeSelectedItemKey");
            }
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public ObservableCollection<TimeSelectedItem> TimeSelectedList { get; set; }

        private void BtnTime_OnMouseEnter(object sender, MouseEventArgs e)
        {
            this.popupTime.IsOpen = true;
        }

        private void PopupTime_OnClosed(object sender, EventArgs e)
        {
            this.TimeSelectedItemKey = null;
        }
    }

    public class TimeSelectedItem
    {
        public int Key { get; set; }

        public string Vaule { get; set; }
    }
}
