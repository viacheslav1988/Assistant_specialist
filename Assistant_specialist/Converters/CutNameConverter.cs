using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Assistant_specialist.Converters
{
    class CutNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = value as string;
            if (result != null)
            {
                if (result.Split(' ').Length < 3) return result;
                return result.Remove(0, result.IndexOf(' ') + 1);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}