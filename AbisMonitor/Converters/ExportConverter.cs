using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AbisMonitor.UI.Converters
{
    #region ExprotStatusImageConvert
    public class ExprotStatusImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is TaskStatus)
            {
                var state = (TaskStatus)value;
                switch (state)
                {
                    case TaskStatus.Created:
                    case TaskStatus.WaitingForActivation:
                    case TaskStatus.WaitingToRun:
                        return @"/Resources/Images/Wait_16x16.png";
                    case TaskStatus.Running:
                        return @"/Resources/Images/Runing16x16.png";
                    case TaskStatus.Faulted:
                        return @"/Resources/Images/Error_16x16.png";
                    case TaskStatus.Canceled:
                        return @"/Resources/Images/Stop16x16.png";
                    case TaskStatus.RanToCompletion:
                        return @"/Resources/Images/Complete16x16.png";

                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region ExportStatusToStringConvert
    public class ExportStatusToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            if (value is TaskStatus)
            {
                var status = (TaskStatus)value;
                switch (status)
                {
                    case TaskStatus.Created:
                    case TaskStatus.WaitingToRun:
                    case TaskStatus.WaitingForActivation:
                        return Application.Current.FindResource("ReadyToBegin") as string;
                    case TaskStatus.Running:
                        return Application.Current.FindResource("Running") as string;
                    case TaskStatus.Faulted:
                        return Application.Current.FindResource("Failed") as string;
                    case TaskStatus.Canceled:
                        return Application.Current.FindResource("Discontinue") as string;
                    case TaskStatus.RanToCompletion:
                        return Application.Current.FindResource("Success") as string;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region ExportComplateVisibilityConvert

    public class ExportComplateVisibilityConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if (value is TaskStatus)
            {
                var status = (TaskStatus)value;
                switch (status)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                    case TaskStatus.RanToCompletion:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region ExprotComlpateOppositeVisibilityConvert

    public class ExprotComlpateOppositeVisibilityConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;
            if (value is TaskStatus)
            {
                var status = (TaskStatus)value;
                switch (status)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                    case TaskStatus.RanToCompletion:
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Visible;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
