using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Common.Helper;

namespace AbisMonitor.UI.Converters
{
    public class DateTimeFormatConvert:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(value.ToString())) return string.Empty;
            try
            {
                DateTime tempDateTime;
                if (DateTime.TryParse(value.ToString(), out tempDateTime))
                {
                    return DateTimeHelper.FormatDateTimeOfYMDHMS(tempDateTime);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
