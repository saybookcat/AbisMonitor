using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AbisMonitor.UI.Common.Controls.ImageButton
{
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        #region Properties

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton),
                new PropertyMetadata(Double.NaN));

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageButton),
                new PropertyMetadata(Double.NaN));

        public static readonly DependencyProperty MouseOverImageProperty = DependencyProperty.Register(
    "MouseOverImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        //public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        //    "Content", typeof(string), typeof(ImageButton), new PropertyMetadata(default(string)));

        //public new static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        //    "Command", typeof(ICommand), typeof(ImageButton), new PropertyMetadata(default(ICommand)));


        //public new static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        //    "CommandParameter", typeof(Object), typeof(ImageButton), new PropertyMetadata(default(Object)));


        //public new Object CommandParameter
        //{
        //    get { return (ICommand)GetValue(CommandParameterProperty); }
        //    set { SetValue(CommandParameterProperty, value); }
        //}

        //public new ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}

        //public new string Content
        //{
        //    get { return (string)GetValue(ContentProperty); }
        //    set { SetValue(ContentProperty, value); }
        //}

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public ImageSource MouseOverImage
        {
            get { return (ImageSource)GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }



        #endregion
    }
}
