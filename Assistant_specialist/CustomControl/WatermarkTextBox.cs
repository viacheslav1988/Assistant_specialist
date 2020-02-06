using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Assistant_specialist.CustomControl
{
    class WatermarkTextBox : TextBox
    {

        #region Конструкторы

        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
            WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(string.Empty));
            WatermarkFontFamilyProperty = DependencyProperty.Register("WatermarkFontFamily", typeof(FontFamily), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(new FontFamily(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontSizeProperty = DependencyProperty.Register("WatermarkFontSize", typeof(double), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontStretchProperty = DependencyProperty.Register("WatermarkFontStretch", typeof(FontStretch), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontStyleProperty = DependencyProperty.Register("WatermarkFontStyle", typeof(FontStyle), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(FontStyles.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontWeightProperty = DependencyProperty.Register("WatermarkFontWeight", typeof(FontWeight), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(FontWeights.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkForegroundProperty = DependencyProperty.Register("WatermarkForeground", typeof(Brush), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(Brushes.Silver, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender | FrameworkPropertyMetadataOptions.Inherits));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        }

        #endregion

        #region Свойства

        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }
        public static readonly DependencyProperty WatermarkTextProperty;

        public FontFamily WatermarkFontFamily
        {
            get { return (FontFamily)GetValue(WatermarkFontFamilyProperty); }
            set { SetValue(WatermarkFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty WatermarkFontFamilyProperty;

        public double WatermarkFontSize
        {
            get { return (double)GetValue(WatermarkFontSizeProperty); }
            set { SetValue(WatermarkFontSizeProperty, value); }
        }
        public static readonly DependencyProperty WatermarkFontSizeProperty;

        public FontStretch WatermarkFontStretch
        {
            get { return (FontStretch)GetValue(WatermarkFontStretchProperty); }
            set { SetValue(WatermarkFontStretchProperty, value); }
        }
        public static readonly DependencyProperty WatermarkFontStretchProperty;

        public FontStyle WatermarkFontStyle
        {
            get { return (FontStyle)GetValue(WatermarkFontStyleProperty); }
            set { SetValue(WatermarkFontStyleProperty, value); }
        }
        public static readonly DependencyProperty WatermarkFontStyleProperty;

        public FontWeight WatermarkFontWeight
        {
            get { return (FontWeight)GetValue(WatermarkFontWeightProperty); }
            set { SetValue(WatermarkFontWeightProperty, value); }
        }
        public static readonly DependencyProperty WatermarkFontWeightProperty;

        public Brush WatermarkForeground
        {
            get { return (Brush)GetValue(WatermarkForegroundProperty); }
            set { SetValue(WatermarkForegroundProperty, value); }
        }
        public static readonly DependencyProperty WatermarkForegroundProperty;

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty;

        #endregion

    }
}