using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Assistant_specialist.Converters
{
    class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string)
            {
                if (Enum.IsDefined(value.GetType(), value))
                {
                    try { return Enum.Parse(value.GetType(), (string)parameter).Equals(value); }
                    catch { }
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string)
            {
                try { return Enum.Parse(targetType, (string)parameter); }
                catch { }
            }
            return Binding.DoNothing;
        }
    }
}