using Assistant_specialist.Tools;
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
    class AmountInWordsDecimalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal) return ((decimal)value).AmountInWords();
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            string current = value as string;
            if (current != null)
            {
                decimal result;
                current = current.OnlyNumbersToConvert();
                if (string.IsNullOrEmpty(current)) return decimal.Zero;
                else if (decimal.TryParse(current, out result)) return result;
                else return Binding.DoNothing;
            }
            else return Binding.DoNothing;
        }
    }
}