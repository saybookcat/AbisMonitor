using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace AbisMonitor.UI.Common.Controls.PagerControl
{
    public class PageLineCountSelectValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 1;
            if (value is System.Windows.Controls.ComboBoxItem)
            {
                var item = value as System.Windows.Controls.ComboBoxItem;
                return item.Content;
            }
            return 1;
        }
    }
}
