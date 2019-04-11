using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bachup.Helpers
{
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "--/--/----";

            DateTime returnVal;

            if (DateTime.TryParse(value.ToString(), out returnVal))
            {
                if (returnVal != DateTime.MinValue)
                    return returnVal;
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

            DateTime val;
            if (value.ToString() == "--/--/----")
                return DateTime.MinValue;

            if (DateTime.TryParse(value.ToString(), out val))
                return val;
            else
                return DateTime.MinValue;
        }
    }
}
