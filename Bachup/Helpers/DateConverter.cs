using System;
using System.Globalization;
using System.Windows.Data;

namespace Bachup.Helpers
{
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "--/--/----";

            if (DateTime.TryParse(value.ToString(), out DateTime returnVal))
            {
                if (returnVal != DateTime.MinValue)
                {
                    return returnVal.ToString("MM/dd/yyyy h:mm tt");
                }
                else
                    return "--/--/----";
            }
            else
                return "--/--/----";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DateTime.MinValue;

            if (value.ToString() == "--/--/----")
                return DateTime.MinValue;

            if (DateTime.TryParse(value.ToString(), out DateTime val))
                return val;
            else
                return DateTime.MinValue;
        }
    }
}
