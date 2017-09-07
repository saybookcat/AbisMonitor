using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace AbisMonitor.UI.Converters
{
    public class HighlightIETitleConverter : IValueConverter
    {
        public Brush DefaultForeBrush { get; set; }
        public Brush ForeBrush { get; set; }
        public Brush DefaultBackBrush { get; set; }
        public Brush BackBrush { get; set; }

        public HighlightIETitleConverter()
        {
            BackBrush = new SolidColorBrush(Colors.Blue);
            DefaultForeBrush = new SolidColorBrush(Colors.Black);
            ForeBrush = new SolidColorBrush(Colors.Yellow);
            DefaultBackBrush = new SolidColorBrush(Colors.Transparent);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string brushType = parameter as string;
            if (String.IsNullOrEmpty(brushType))
                return DefaultBackBrush;

            if (!brushType.Equals("Fore"))
                brushType = "Back";

            string msg = value as string;
            if (String.IsNullOrEmpty(msg))
            {
                if (brushType.Equals("Fore"))
                    return DefaultForeBrush;
                else
                    return DefaultBackBrush;
            }

            if (msg.StartsWith("INFORMATION ELEMENT"))
            {
                if (brushType.Equals("Fore"))
                    return ForeBrush;
                else
                    return BackBrush;
            }
            else
            {
                if (brushType.Equals("Fore"))
                    return DefaultForeBrush;
                else
                    return DefaultBackBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
