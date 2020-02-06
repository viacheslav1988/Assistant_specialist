using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Assistant_specialist.Converters
{
    class FindElementNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ContentControl parent = value as ContentControl;
            string name = parameter as string;
            FrameworkElement result = null;
            if (parent != null & name != null) result = FindElement(parent, name);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public FrameworkElement FindElement(FrameworkElement parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name)) return null;
            if (parent.Name == name) return parent;
            FrameworkElement result = null;
            parent.ApplyTemplate();
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                result = FindElement(child, name);
                if (result != null) break;
            }
            return result;
        }
    }
}
