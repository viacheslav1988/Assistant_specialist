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
    class SelectedCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius & parameter != null & parameter is string)
            {
                double topLeft = ((CornerRadius)value).TopLeft;
                double topRight = ((CornerRadius)value).TopRight;
                double bottomRight = ((CornerRadius)value).BottomRight;
                double bottomLeft = ((CornerRadius)value).BottomLeft;
                switch ((string)parameter)
                {
                    case "BottomLeft": return new CornerRadius(0.0, 0.0, 0.0, bottomLeft);
                    case "Left": return new CornerRadius(topLeft, 0.0, 0.0, bottomLeft);
                    case "TopLeft": return new CornerRadius(topLeft, 0.0, 0.0, 0.0);
                    case "Top": return new CornerRadius(topLeft, topRight, 0.0, 0.0);
                    case "TopRight": return new CornerRadius(0.0, topRight, 0.0, 0.0);
                    case "Right": return new CornerRadius(0.0, topRight, bottomRight, 0.0);
                    case "BottomRight": return new CornerRadius(0.0, 0.0, bottomRight, 0.0);
                    case "Bottom": return new CornerRadius(0.0, 0.0, bottomRight, bottomLeft);
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}