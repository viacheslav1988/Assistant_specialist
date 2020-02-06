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
    class AddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string current = value as string;
            if (current != null)
            {
                if (!Regex.IsMatch(current, @"^.*([,]\s)(\D+\1){2}\d+(\s?[/]\s?\d+)?\1\d+[;].*$")) return Binding.DoNothing;
                current = current.Replace(", ", "|");
                int from = current.IndexOf('|');
                int to = current.LastIndexOf(';');
                string[] CutAddress = current.Substring(from + 1, to - from - 1).Split('|');
                StringBuilder result = new StringBuilder("ул. ");
                result.Append(CutAddress[1]).Append(", д. ").Append(CutAddress[2]).Append(", кв. ").Append(CutAddress[3]);
                result.Append((char)13).Append((char)10).Append("г.").Append(CutAddress[0]).Append(", Свердловская обл., 620000");
                return result.ToString();
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}