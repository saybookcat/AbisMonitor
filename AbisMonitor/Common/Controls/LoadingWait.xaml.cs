using System;
using System.Collections.Generic;
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
    /// LoadingWait.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWait : UserControl
    {
        public string LodingText
        {
            set { txt_loading.Text = value; }
        }

        public Visibility CancelButtionVisibility
        {
            set { this.BtnCancelQuery.Visibility = value; }
        }

        #region Data
        private readonly System.Windows.Threading.DispatcherTimer _animationTimer;
        #endregion

        #region Constructor

        public LoadingWait()
        {
            InitializeComponent();

            _animationTimer = new System.Windows.Threading.DispatcherTimer(
                System.Windows.Threading.DispatcherPriority.ContextIdle, Dispatcher);
            _animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 90);
            this.IsVisibleChanged += HandleVisibleChanged;

        }

        #endregion

        #region Private Methods
        private void Start()
        {
            if (_animationTimer.IsEnabled) return;
            this.LodingText = "正在加载...";
            var convertFromString = System.Windows.Media.ColorConverter.ConvertFromString("#FFE3953D");
            if (convertFromString != null)
            {
                var color = (System.Windows.Media.Color)convertFromString;
                this.txt_loading.Foreground = new SolidColorBrush(color);
            }
            _animationTimer.Tick += HandleAnimationTick;
            _animationTimer.Start();
        }

        private void Stop()
        {
            if (!_animationTimer.IsEnabled) return;
            this.LodingText = "正在取消...";
            this.txt_loading.Foreground = Brushes.OrangeRed;
            _animationTimer.Stop();
            _animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
        }

        private void SetPosition(Ellipse ellipse, double offset,
            double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50.0
                + Math.Sin(offset + posOffSet * step) * 50.0);

            ellipse.SetValue(Canvas.TopProperty, 50
                + Math.Cos(offset + posOffSet * step) * 50.0);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void HandleVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible && this.Visibility == Visibility.Visible)
                Start();
            else
                Stop();
        }
        #endregion


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Stop();
            if (CancelQueryCommand != null)
            {
                CancelQueryCommand.Execute(null);
            }
        }

        public static readonly DependencyProperty CancelQueryCommandProperty = DependencyProperty.Register(
            "CancelQueryCommand", typeof(ICommand), typeof(LoadingWait), new PropertyMetadata(default(ICommand)));

        public ICommand CancelQueryCommand
        {
            get { return (ICommand)GetValue(CancelQueryCommandProperty); }
            set { SetValue(CancelQueryCommandProperty, value); }
        }
    }
}
