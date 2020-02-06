using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assistant_specialist.CustomControl
{
    public class CustomButton : Button
    {

        #region Конструкторы

        static CustomButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata(typeof(CustomButton)));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomButton), new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        }

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty;

        #endregion

    }
}