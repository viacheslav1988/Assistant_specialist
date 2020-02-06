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
    class FindElementNameListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ContentControl parent = value as ContentControl;
            string[] names = parameter is string ? ((string)parameter).Split(',') : null;
            List<FrameworkElement> result = null;
            if (parent != null & names != null)
            {
                result = new List<FrameworkElement>();
                foreach(string name in names)
                {
                    FrameworkElement element = FindElement(parent, name);
                    if (element != null) result.Add(element);
                }
            }
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
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                result = FindElement(child, name);
                if (result != null) break;
            }
            return result;
        }
    }
}