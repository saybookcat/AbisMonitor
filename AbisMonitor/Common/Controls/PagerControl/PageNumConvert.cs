using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace AbisMonitor.UI.Common.Controls.PagerControl
{
    public class PageNumConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            int pageNum = 1;
            if (value == null) return pageNum;
            if (!int.TryParse(value.ToString(), out pageNum)) return pageNum;
            if (pageNum < 1)
                pageNum = 1;
            return pageNum;
        }
    }
}
