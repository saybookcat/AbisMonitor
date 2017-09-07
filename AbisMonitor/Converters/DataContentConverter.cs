using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Common.Helper;

namespace AbisMonitor.UI.Converters
{
    public class DataContentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return string.Empty;
            if (values.Length != 2) return string.Empty;
            var data = values[0] as byte[];
            if (data == null || data.Length == 0) return string.Empty;
            //double width;
            //if (double.TryParse(values[1].ToString(), out width) == false) return string.Empty;

            return DataContentHelper.DataContentConvert(data);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
