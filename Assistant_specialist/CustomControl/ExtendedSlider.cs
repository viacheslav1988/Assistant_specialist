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
using Assistant_specialist.Tools;

namespace Assistant_specialist.CustomControl
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplatePart(Name = "PART_ButtonMinus", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_ButtonPlus", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Slider", Type = typeof(Slider))]
    class ExtendedSlider : Control
    {

        #region Поля и перечисления

        private Button PART_ButtonMinus;
        private TextBlock PART_TextBlock;
        private Button PART_ButtonPlus;
        private Slider PART_Slider;
        private enum Operation { Minus, Plus }

        #endregion

        #region Свойства зависимости

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty;

        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty;

        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty;

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty;

        public decimal Step
        {
            get { return (decimal)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty;

        #endregion

        #region Конструкторы

        static ExtendedSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedSlider), new FrameworkPropertyMetadata(typeof(ExtendedSlider)));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ExtendedSlider), new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(ExtendedSlider), new FrameworkPropertyMetadata(0.0m));
            MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(ExtendedSlider), new FrameworkPropertyMetadata(1.0m));
            ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(ExtendedSlider), new FrameworkPropertyMetadata(1.0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));
            StepProperty = DependencyProperty.Register("Step", typeof(decimal), typeof(ExtendedSlider), new FrameworkPropertyMetadata(0.1m));
        }

        public ExtendedSlider()
        {
            IsEnabledChanged += (sender, args) => ChangeVisualState(true);
            MouseEnter += (sender, args) => ChangeVisualState(true);
            MouseLeave += (sender, args) => ChangeVisualState(true);
        }

        #endregion

        #region Методы

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            #region Определение элементов из шаблона и применение стиля

            #region PART_ButtonMinus

            PART_ButtonMinus = GetTemplateChild("PART_ButtonMinus") as Button;
            string PART_ButtonMinusStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:CustomControl=""clr-namespace:Assistant_specialist.CustomControl"" TargetType=""{x:Type Button}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Content"" Value=""&#10134;""/><Setter Property=""Focusable"" Value=""False""/><Setter Property=""HorizontalContentAlignment"" Value=""{Binding HorizontalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""VerticalContentAlignment"" Value=""{Binding VerticalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Background"" Value=""{Binding Background, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""BorderBrush"" Value=""{Binding BorderBrush, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""BorderThickness"" Value=""0,0,1,0""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type Button}""><Grid><VisualStateManager.VisualStateGroups><VisualStateGroup Name=""CommonStates""><VisualStateGroup.Transitions><VisualTransition GeneratedDuration=""0:0:0.2""/></VisualStateGroup.Transitions><VisualState Name=""Normal""/><VisualState Name=""MouseOver""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Border"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.MouseOver.Background}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Pressed""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Border"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.Pressed.Background}}"" Duration=""0""/><ColorAnimation Storyboard.TargetName=""PART_Content"" Storyboard.TargetProperty=""(TextBlock.Foreground).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.Pressed.Foreground}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Disabled""/></VisualStateGroup><VisualStateGroup Name=""FocusStates""><VisualState Name=""Focused""/><VisualState Name=""Unfocused""/></VisualStateGroup><VisualStateGroup Name=""ValidationStates""><VisualState Name=""Valid""/><VisualState Name=""InvalidFocused""/><VisualState Name=""InvalidUnfocused""/></VisualStateGroup></VisualStateManager.VisualStateGroups><Border x:Name=""PART_Border"" Background=""{TemplateBinding Background}"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" CornerRadius=""{Binding CornerRadius, RelativeSource={RelativeSource AncestorType={x:Type CustomControl:ExtendedSlider}}, Converter={StaticResource SelectedCornerRadiusConverter}, ConverterParameter='TopLeft'}""/><ContentPresenter x:Name=""PART_Content"" Margin=""{TemplateBinding Padding}"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""/></Grid></ControlTemplate></Setter.Value></Setter></Style>";
            if (PART_ButtonMinus != null) PART_ButtonMinus.Style = PART_ButtonMinusStyle.XamlStringToObject<Style>();

            #endregion

            #region PART_TextBlock

            PART_TextBlock = GetTemplateChild("PART_TextBlock") as TextBlock;
            string PART_TextBlockStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""{x:Type TextBlock}""><Setter Property=""Background"" Value=""{StaticResource ExtendedSlider.Static.Background}""/><Setter Property=""TextAlignment"" Value=""Center""/><Setter Property=""Padding"" Value=""0,2,0,0""/></Style>";
            if (PART_TextBlock != null) PART_TextBlock.Style = PART_TextBlockStyle.XamlStringToObject<Style>();

            #endregion

            #region PART_ButtonPlus

            PART_ButtonPlus = GetTemplateChild("PART_ButtonPlus") as Button;
            string PART_ButtonPlusStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:CustomControl=""clr-namespace:Assistant_specialist.CustomControl"" TargetType=""{x:Type Button}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Content"" Value=""&#10133;""/><Setter Property=""Focusable"" Value=""False""/><Setter Property=""HorizontalContentAlignment"" Value=""{Binding HorizontalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""VerticalContentAlignment"" Value=""{Binding VerticalContentAlignment, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Background"" Value=""{Binding Background, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""BorderBrush"" Value=""{Binding BorderBrush, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""BorderThickness"" Value=""1,0,0,0""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type Button}""><Grid><VisualStateManager.VisualStateGroups><VisualStateGroup Name=""CommonStates""><VisualStateGroup.Transitions><VisualTransition GeneratedDuration=""0:0:0.2""/></VisualStateGroup.Transitions><VisualState Name=""Normal""/><VisualState Name=""MouseOver""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Border"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.MouseOver.Background}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Pressed""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Border"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.Pressed.Background}}"" Duration=""0""/><ColorAnimation Storyboard.TargetName=""PART_Content"" Storyboard.TargetProperty=""(TextBlock.Foreground).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.Pressed.Foreground}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Disabled""/></VisualStateGroup><VisualStateGroup Name=""FocusStates""><VisualState Name=""Focused""/><VisualState Name=""Unfocused""/></VisualStateGroup><VisualStateGroup Name=""ValidationStates""><VisualState Name=""Valid""/><VisualState Name=""InvalidFocused""/><VisualState Name=""InvalidUnfocused""/></VisualStateGroup></VisualStateManager.VisualStateGroups><Border x:Name=""PART_Border"" Background=""{TemplateBinding Background}"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" CornerRadius=""{Binding CornerRadius, RelativeSource={RelativeSource AncestorType={x:Type CustomControl:ExtendedSlider}}, Converter={StaticResource SelectedCornerRadiusConverter}, ConverterParameter='TopRight'}""/><ContentPresenter x:Name=""PART_Content"" Margin=""{TemplateBinding Padding}"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""/></Grid></ControlTemplate></Setter.Value></Setter></Style>";
            if (PART_ButtonPlus != null) PART_ButtonPlus.Style = PART_ButtonPlusStyle.XamlStringToObject<Style>();

            #endregion

            #region PART_Slider

            PART_Slider = GetTemplateChild("PART_Slider") as Slider;
            string PART_SliderStyle = @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:CustomControl=""clr-namespace:Assistant_specialist.CustomControl"" TargetType=""{x:Type Slider}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Focusable"" Value=""False""/><Setter Property=""LargeChange"" Value=""{Binding Step, RelativeSource={RelativeSource AncestorType={x:Type CustomControl:ExtendedSlider}}}""/><Setter Property=""Padding"" Value=""10,0""/><Setter Property=""Background"" Value=""{Binding Background, RelativeSource={RelativeSource TemplatedParent}}""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type Slider}""><ControlTemplate.Resources><Style x:Key=""RepeatButtonStyle"" TargetType=""{x:Type RepeatButton}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Focusable"" Value=""False""/><Setter Property=""Background"" Value=""{StaticResource ExtendedSlider.TransparentBrush}""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type RepeatButton}""><Border Background=""{TemplateBinding Background}""/></ControlTemplate></Setter.Value></Setter></Style><Style x:Key=""ThumbStyle"" TargetType=""{x:Type Thumb}""><Setter Property=""OverridesDefaultStyle"" Value=""True""/><Setter Property=""Width"" Value=""10""/><Setter Property=""Height"" Value=""10""/><Setter Property=""Template""><Setter.Value><ControlTemplate TargetType=""{x:Type Thumb}""><Image x:Name=""PART_Image"" Width=""{TemplateBinding Width}"" Height=""{TemplateBinding Height}""><VisualStateManager.VisualStateGroups><VisualStateGroup Name=""CommonStates""><VisualStateGroup.Transitions><VisualTransition GeneratedDuration=""0:0:0.2""/></VisualStateGroup.Transitions><VisualState Name=""Normal""/><VisualState Name=""MouseOver""/><VisualState Name=""Pressed""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Image"" Storyboard.TargetProperty=""(Image.Source).(DrawingImage.Drawing).(DrawingGroup.Children)[1].(GeometryDrawing.Brush).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.Thumb.Pressed.Brush}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Disabled""/></VisualStateGroup><VisualStateGroup Name=""FocusStates""><VisualState Name=""Focused""/><VisualState Name=""Unfocused""/></VisualStateGroup><VisualStateGroup Name=""ValidationStates""><VisualState Name=""Valid""/><VisualState Name=""InvalidFocused""/><VisualState Name=""InvalidUnfocused""/></VisualStateGroup></VisualStateManager.VisualStateGroups><Image.Source><DrawingImage><DrawingImage.Drawing><DrawingGroup><GeometryDrawing Brush=""{StaticResource ExtendedSlider.Thumb.Static.Brush0}"" Geometry=""M0,5 C0,2.239 2.239,0 5,0 C7.761,0 10,2.239 10,5 C10,7.761 7.761,10 5,10 C2.239,10 0,7.761 0,5 Z""/><GeometryDrawing Brush=""{StaticResource ExtendedSlider.Thumb.Static.Brush1}"" Geometry=""M2,5 C2,3.343 3.343,2 5,2 C6.657,2 8,3.343 8,5 C8,6.657 6.657,8 5,8 C3.343,8 2,6.657 2,5 Z""/></DrawingGroup></DrawingImage.Drawing></DrawingImage></Image.Source></Image></ControlTemplate></Setter.Value></Setter></Style></ControlTemplate.Resources><Border x:Name=""PART_Border"" Background=""{TemplateBinding Background}"" CornerRadius=""{Binding CornerRadius, RelativeSource={RelativeSource AncestorType={x:Type CustomControl:ExtendedSlider}}, Converter={StaticResource SelectedCornerRadiusConverter}, ConverterParameter='Bottom'}""><VisualStateManager.VisualStateGroups><VisualStateGroup Name=""CommonStates""><VisualStateGroup.Transitions><VisualTransition GeneratedDuration=""0:0:0.2""/></VisualStateGroup.Transitions><VisualState Name=""Normal""/><VisualState Name=""MouseOver""><Storyboard><ColorAnimation Storyboard.TargetName=""PART_Border"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"" To=""{Binding Color, Source={StaticResource ExtendedSlider.MouseOver.Background}}"" Duration=""0""/></Storyboard></VisualState><VisualState Name=""Disabled""/></VisualStateGroup><VisualStateGroup Name=""FocusStates""><VisualState Name=""Focused""/><VisualState Name=""Unfocused""/></VisualStateGroup><VisualStateGroup Name=""ValidationStates""><VisualState Name=""Valid""/><VisualState Name=""InvalidFocused""/><VisualState Name=""InvalidUnfocused""/></VisualStateGroup></VisualStateManager.VisualStateGroups><Grid Margin=""{TemplateBinding Padding}""><Border Margin=""5,0"" Background=""{StaticResource ExtendedSlider.Track.Static.Brush}"" Height=""3"" CornerRadius=""1.5""/><Track x:Name=""PART_Track""><Track.DecreaseRepeatButton><RepeatButton Style=""{StaticResource RepeatButtonStyle}"" Command=""{x:Static Slider.DecreaseLarge}""/></Track.DecreaseRepeatButton><Track.Thumb><Thumb Style=""{StaticResource ThumbStyle}""/></Track.Thumb><Track.IncreaseRepeatButton><RepeatButton Style=""{StaticResource RepeatButtonStyle}"" Command=""{x:Static Slider.IncreaseLarge}""/></Track.IncreaseRepeatButton></Track></Grid></Border></ControlTemplate></Setter.Value></Setter></Style>";
            if (PART_Slider != null) PART_Slider.Style = PART_SliderStyle.XamlStringToObject<Style>();

            #endregion

            #endregion

            #region Поведение PART_ButtonMinus

            if (PART_ButtonMinus != null)
            {
                PART_ButtonMinus.Click += (sender, args) =>
                {
                    decimal result = TrimByNumber(Value, Step, Operation.Minus);
                    if (result < Minimum) Value = Minimum;
                    else Value = result;
                };
            }

            #endregion

            #region Поведение PART_TextBlock

            if (PART_TextBlock != null)
            {
                PART_TextBlock.SetBinding(TextBlock.TextProperty, new Binding("Value")
                {
                    Source = this,
                    StringFormat = @"{0:P0}"
                });
            }

            #endregion

            #region Поведение PART_ButtonPlus

            if (PART_ButtonPlus != null)
            {
                PART_ButtonPlus.Click += (sender, args) =>
                {
                    decimal result = TrimByNumber(Value, Step, Operation.Plus);
                    if (result > Maximum) Value = Maximum;
                    else Value = result;
                };
            }

            #endregion

            #region Поведение PART_Slider

            if (PART_Slider != null)
            {
                PART_Slider.SetBinding(Slider.MinimumProperty, new Binding("Minimum")
                {
                    Source = this,
                    Mode = BindingMode.TwoWay
                });
                PART_Slider.SetBinding(Slider.MaximumProperty, new Binding("Maximum")
                {
                    Source = this,
                    Mode = BindingMode.TwoWay
                });
                PART_Slider.SetBinding(Slider.ValueProperty, new Binding("Value")
                {
                    Source = this
                });
            }

            #endregion

            ChangeVisualState(false);
        }

        private void ChangeVisualState(bool useTransitions)
        {
            if (!IsEnabled) VisualStateManager.GoToState(this, "Disabled", useTransitions);
            else if (IsMouseOver) VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            else VisualStateManager.GoToState(this, "Normal", useTransitions);
        }

        private decimal TrimByNumber(decimal value, decimal step, Operation operation)
        {
            decimal number = step - Math.Truncate(step);
            int count = 0;
            while (number - Math.Truncate(number) != 0m)
            {
                number *= 10m;
                count++;
            }
            decimal round = 1m;
            for (int i = 0; i < count; i++) round *= 10m;
            switch (operation)
            {
                default:
                case Operation.Minus:
                    if (Math.Truncate(value * round) == (value * round)) return Math.Truncate(value * round) / round - step;
                    else return Math.Truncate(value * round) / round;
                case Operation.Plus: return Math.Truncate(value * round) / round + step;
            }
        }

        #endregion

    }
}
