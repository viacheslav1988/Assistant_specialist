using Assistant_specialist.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xaml;

namespace Assistant_specialist.CustomControl
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Watermark", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Button", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    class WatermarkDatePicker : Control
    {

        #region Конвертер DateTimeToStringConverter

        private class DateTimeToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null) return string.Empty;
                else return ((DateTime)value).ToString(DateFormat);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                DateTime result;
                if ((string)value == string.Empty) return null;
                else if (DateTime.TryParse((string)value, out result)) return result;
                else return DependencyProperty.UnsetValue;
            }
        }

        #endregion

        #region Константы

        private const string ElementTextBox = "PART_TextBox";
        private const string ElementTextBlock = "PART_Watermark";
        private const string ElementButton = "PART_Button";
        private const string ElementPopup = "PART_Popup";
        private const string DateFormat = "dd.MM.yyyy";

        #endregion

        #region Поля

        private TextBox PART_TextBox;
        private TextBlock PART_Watermark;
        private Button PART_Button;
        private Popup PART_Popup;
        private Calendar PART_Calendar;
        private DispatcherTimer timer;

        #endregion

        #region Свойства зависимости

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty;

        public bool IsMainFocused
        {
            get { return (bool)GetValue(IsMainFocusedProperty); }
            set { SetValue(IsMainFocusedProperty, value); }
        }
        public static readonly DependencyProperty IsMainFocusedProperty;

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateProperty;

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

        #endregion

        #region Маршрутизируемые события

        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        public static readonly RoutedEvent SelectedDateChangedEvent;

        #endregion

        #region Конструкторы

        static WatermarkDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(typeof(WatermarkDatePicker)));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            IsMainFocusedProperty = DependencyProperty.Register("IsMainFocused", typeof(bool), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(false, (obj, args) => { if (obj is WatermarkDatePicker) ((WatermarkDatePicker)obj).ChangeVisualState(true); }));
            SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(null, (obj, args) => ((WatermarkDatePicker)obj).RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent))));
            WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(string.Empty));
            WatermarkFontFamilyProperty = DependencyProperty.Register("WatermarkFontFamily", typeof(FontFamily), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(new FontFamily(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontSizeProperty = DependencyProperty.Register("WatermarkFontSize", typeof(double), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontStretchProperty = DependencyProperty.Register("WatermarkFontStretch", typeof(FontStretch), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontStyleProperty = DependencyProperty.Register("WatermarkFontStyle", typeof(FontStyle), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(FontStyles.Italic, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkFontWeightProperty = DependencyProperty.Register("WatermarkFontWeight", typeof(FontWeight), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(FontWeights.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            WatermarkForegroundProperty = DependencyProperty.Register("WatermarkForeground", typeof(Brush), typeof(WatermarkDatePicker), new FrameworkPropertyMetadata(Brushes.Silver, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender | FrameworkPropertyMetadataOptions.Inherits));

            SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(WatermarkDatePicker));
        }

        public WatermarkDatePicker()
        {
            IsEnabledChanged += (sender, args) => ChangeVisualState(true);

            MouseEnter += (sender, args) => ChangeVisualState(true);
            MouseLeave += (sender, args) => ChangeVisualState(true);

            #region Инициализация, применение стиля и поведение PART_Calendar

            PART_Calendar = new Calendar();

            string PART_CalendarStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""{x:Type Calendar}""><Setter Property=""Margin"" Value=""0,0,5,5""/><Setter Property=""Effect""><Setter.Value><DropShadowEffect Opacity=""0.4"" ShadowDepth=""1.5"" Direction=""300""/></Setter.Value></Setter></Style>";
            PART_Calendar.Style = PART_CalendarStyle.XamlStringToObject<Style>();

            PART_Calendar.SetBinding(Calendar.SelectedDateProperty, new Binding("SelectedDate") { Source = this, Mode = BindingMode.TwoWay });
            PART_Calendar.SetBinding(Calendar.DisplayDateProperty, new Binding("SelectedDate") { Source = this, Mode = BindingMode.OneWay });

            PART_Calendar.Loaded += (sender, args) => { if (sender is Calendar) ((Calendar)sender).DisplayDate = SelectedDate == null ? DateTime.Today : (DateTime)SelectedDate; };
            PART_Calendar.SelectedDatesChanged += (sender, args) =>
            {
                if (PART_Popup != null) PART_Popup.IsOpen = false;
                if (timer != null) timer.Stop();
            };
            PART_Calendar.PreviewMouseRightButtonDown += (sender, args) => { if (PART_Popup != null) PART_Popup.IsOpen = false; };

            #endregion

        }

        #endregion

        #region Методы

        public void Clear()
        {
            if (SelectedDate != null) SelectedDate = null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            #region Определение элементов из шаблона и применение стиля

            PART_TextBox = GetTemplateChild(ElementTextBox) as TextBox;
            string PART_TextBoxStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""{x:Type TextBox}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""AllowDrop"" Value=""True""/><Setter Property=""Validation.ErrorTemplate"" Value=""{x:Null}""/><Setter Property=""FocusVisualStyle"" Value=""{x:Null}""/><Setter Property=""HorizontalAlignment"" Value=""{Binding HorizontalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""VerticalAlignment"" Value=""{Binding VerticalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Padding"" Value=""{Binding Padding, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""ContextMenu"" Value=""{Binding ContextMenu, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""CaretBrush"" Value=""White""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type TextBox}""><ScrollViewer x:Name=""PART_ContentHost""/></ControlTemplate></Setter.Value></Setter></Style>";
            if (PART_TextBox != null) PART_TextBox.Style = PART_TextBoxStyle.XamlStringToObject<Style>();

            PART_Watermark = GetTemplateChild(ElementTextBlock) as TextBlock;
            string PART_WatermarkStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""{x:Type TextBlock}""><Setter Property=""Visibility"" Value=""Collapsed""/><Setter Property=""Text"" Value=""{Binding WatermarkText, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""FontFamily"" Value=""{Binding WatermarkFontFamily, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""FontSize"" Value=""{Binding WatermarkFontSize, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""FontStretch"" Value=""{Binding WatermarkFontStretch, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""FontStyle"" Value=""{Binding WatermarkFontStyle, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""FontWeight"" Value=""{Binding WatermarkFontWeight, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Foreground"" Value=""{Binding WatermarkForeground, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""HorizontalAlignment"" Value=""{Binding HorizontalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""VerticalAlignment"" Value=""{Binding VerticalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Margin"" Value=""{Binding Padding, RelativeSource={RelativeSource TemplatedParent}}""/><Style.Triggers><DataTrigger Binding=""{Binding SelectedDate, RelativeSource={RelativeSource TemplatedParent}}"" Value=""{x:Null}""><Setter Property=""Visibility"" Value=""Visible""/></DataTrigger><DataTrigger Binding=""{Binding IsMainFocused, RelativeSource={RelativeSource TemplatedParent}}"" Value=""True""><Setter Property=""Visibility"" Value=""Collapsed""/></DataTrigger></Style.Triggers></Style>";
            if (PART_Watermark != null) PART_Watermark.Style = PART_WatermarkStyle.XamlStringToObject<Style>();

            PART_Button = GetTemplateChild(ElementButton) as Button;
            string PART_ButtonStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:CustomControl=""clr-namespace:Assistant_specialist.CustomControl"" TargetType=""{x:Type Button}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Focusable"" Value=""False""/><Setter Property=""TextOptions.TextFormattingMode"" Value=""Display""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type Button}""><Grid><VisualStateManager.VisualStateGroups><VisualStateGroup Name=""CommonStates""><VisualState Name=""Normal""/><VisualState Name=""MouseOver""><Storyboard><DoubleAnimation Storyboard.TargetName=""PART_Icon"" Storyboard.TargetProperty=""(Image.Effect).(DropShadowEffect.Opacity)"" To=""1"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Pressed""><Storyboard><DoubleAnimation Storyboard.TargetName=""PART_Icon"" Storyboard.TargetProperty=""(Image.Effect).(DropShadowEffect.Opacity)"" To=""1"" Duration=""0""/><ColorAnimation Storyboard.TargetName=""PART_IconDay"" Storyboard.TargetProperty=""(TextBlock.Foreground).(SolidColorBrush.Color)"" To=""#ffd82d00"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Disabled""/></VisualStateGroup><VisualStateGroup Name=""FocusStates""><VisualState Name=""Focused""/><VisualState Name=""Unfocused""/></VisualStateGroup><VisualStateGroup Name=""ValidationStates""><VisualState Name=""Valid""/><VisualState Name=""InvalidFocused""/><VisualState Name=""InvalidUnfocused""/></VisualStateGroup></VisualStateManager.VisualStateGroups><Image x:Name=""PART_Icon"" Margin=""9,7"" Width=""18"" Height=""21"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" SnapsToDevicePixels=""True""><Image.Resources><SolidColorBrush x:Key=""Brush1"" Color=""#ff000000""/><SolidColorBrush x:Key=""Brush2"" Color=""#ffe1e1e1""/><SolidColorBrush x:Key=""Brush3"" Color=""#ffd82d00""/><LinearGradientBrush x:Key=""Brush4"" StartPoint=""0.5,0"" EndPoint=""0.5,1"" ColorInterpolationMode=""ScRgbLinearInterpolation""><GradientStop Offset=""0.25"" Color=""#ffc5c5c5""/><GradientStop Offset=""0.5"" Color=""#ff8f8f8f""/><GradientStop Offset=""0.75"" Color=""#ffc5c5c5""/></LinearGradientBrush></Image.Resources><Image.Source><DrawingImage><DrawingImage.Drawing><DrawingGroup><GeometryDrawing Brush=""{StaticResource Brush1}"" Geometry=""M16.5,21 L1.5,21 C0.672,21 0,20.328 0,19.5 L0,6 L18,6 L18,19.5 C18,20.328 17.328,21 16.5,21 Z""/><GeometryDrawing Brush=""{StaticResource Brush2}"" Geometry=""M16.5,20 L1.5,20 C0.672,20 0,19.328 0,18.5 L0,6 L18,6 L18,18.5 C18,19.328 17.328,20 16.5,20 Z""/><GeometryDrawing Brush=""{StaticResource Brush3}"" Geometry=""M12,3 L6,3 C5.448,3 5,2.552 5,2 C5,1.448 5.448,1 6,1 L12,1 C12.552,1 13,1.448 13,2 C13,2.552 12.552,3 12,3 Z M16.5,0 L1.5,0 C0.672,0 0,0.672 0,1.5 L0,6 L18,6 L18,1.5 C18,0.672 17.328,0 16.5,0 Z""/><GeometryDrawing Brush=""{StaticResource Brush4}"" Geometry=""M7,8 L5,8 L5,4 L7,4 L7,8 Z""/><GeometryDrawing Brush=""{StaticResource Brush4}"" Geometry=""M4,8 L2,8 L2,4 L4,4 L4,8 Z""/><GeometryDrawing Brush=""{StaticResource Brush4}"" Geometry=""M10,8 L8,8 L8,4 L10,4 L10,8 Z""/><GeometryDrawing Brush=""{StaticResource Brush4}"" Geometry=""M13,8 L11,8 L11,4 L13,4 L13,8 Z""/><GeometryDrawing Brush=""{StaticResource Brush4}"" Geometry=""M16,8 L14,8 L14,4 L16,4 L16,8 Z""/></DrawingGroup></DrawingImage.Drawing></DrawingImage></Image.Source><Image.Effect><DropShadowEffect BlurRadius=""7"" Color=""Yellow"" Opacity=""0"" ShadowDepth=""0""/></Image.Effect></Image><TextBlock x:Name=""PART_IconDay"" Text=""{Binding SelectedDate.Day, FallbackValue='31', RelativeSource={RelativeSource AncestorType={x:Type CustomControl:WatermarkDatePicker}}}"" FontSize=""11"" Foreground=""Black"" Margin=""0,13,0,0"" HorizontalAlignment=""Center""/></Grid></ControlTemplate></Setter.Value></Setter></Style>";
            if (PART_Button != null) PART_Button.Style = PART_ButtonStyle.XamlStringToObject<Style>();

            PART_Popup = GetTemplateChild(ElementPopup) as Popup;
            string PART_PopupStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""{x:Type Popup}""><Setter Property=""PlacementTarget"" Value=""{Binding ElementName=PART_Button}""/><Setter Property=""HorizontalOffset"" Value=""-149""/><Setter Property=""Placement"" Value=""Bottom""/><Setter Property=""AllowsTransparency"" Value=""True""/><Setter Property=""StaysOpen"" Value=""False""/></Style>";
            if (PART_Popup != null) PART_Popup.Style = PART_PopupStyle.XamlStringToObject<Style>();

            #endregion

            #region Поведение PART_TextBox

            if (PART_TextBox != null)
            {
                PART_TextBox.LostFocus += (sender, args) =>
                {
                    PART_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    if (timer != null) timer.Stop();
                };

                PART_TextBox.PreviewKeyDown += (sender, args) =>
                {
                    if (Keyboard.IsKeyDown(Key.Enter))
                    {
                        PART_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        if (timer != null) timer.Stop();
                    }
                };

                PART_TextBox.TextChanged += (sender, args) =>
                {
                    if (timer == null)
                    {
                        timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(2.0);
                        timer.Tick += (s, a) =>
                        {
                            DispatcherTimer currentTimer = s as DispatcherTimer;
                            if (currentTimer != null)
                            {
                                TextBox textBox = currentTimer.Tag as TextBox;
                                if (textBox != null)
                                {
                                    if (Regex.IsMatch(textBox.Text, @"^\s*\d{2}[.,]\d{2}[.,]\d{4}\s*$"))
                                    {
                                        textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                                    }
                                }
                                currentTimer.Stop();
                            }
                        };
                    }
                    timer.Stop();
                    timer.Tag = sender;
                    timer.Start();
                };

                BindingOperations.SetBinding(this, IsMainFocusedProperty, new Binding("IsFocused") { Source = PART_TextBox });

                BindingOperations.SetBinding(PART_TextBox, TextBox.TextProperty, new Binding("SelectedDate")
                {
                    Source = this,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                    NotifyOnValidationError = true,
                    Converter = new DateTimeToStringConverter()
                });
                Validation.AddErrorHandler(PART_TextBox, (sender, args) =>
                {
                    Validation.ClearInvalid(PART_TextBox.GetBindingExpression(TextBox.TextProperty)); ;
                    if (SelectedDate == null) ((TextBox)sender).Text = string.Empty;
                    else
                    {
                        ((TextBox)sender).Text = ((DateTime)SelectedDate).ToString(DateFormat);
                        ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                    }
                });
            }

            #endregion

            #region Поведение PART_Button

            if (PART_Button != null)
            {
                PART_Button.Click += (sender, args) =>
                {
                    if (PART_Popup != null) PART_Popup.IsOpen = true;
                };
            }

            #endregion

            #region Поведение PART_Popup

            if (PART_Popup != null)
            {
                PART_Popup.Child = PART_Calendar;
            }

            #endregion

            ChangeVisualState(false);
        }

        private void ChangeVisualState(bool useTransitions)
        {
            if (!IsEnabled) VisualStateManager.GoToState(this, "Disabled", useTransitions);
            else if (IsMouseOver) VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            else VisualStateManager.GoToState(this, "Normal", useTransitions);

            if (IsMainFocused) VisualStateManager.GoToState(this, "Focused", useTransitions);
            else VisualStateManager.GoToState(this, "Unfocused", useTransitions);
        }

        #endregion

    }
}