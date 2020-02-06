using Assistant_specialist.CustomControl;
using Assistant_specialist.Models.PositiveResponseM;
using Assistant_specialist.Tools;
using Assistant_specialist.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xaml;

namespace Assistant_specialist.ViewModels.PositiveResponseVM
{
    class PositiveResponseViewModel : ViewModelBase, IDataErrorInfo
    {

        #region Поля

        private Point? point;

        #region Словарь таймеров

        private Dictionary<string, DispatcherTimer> timers;

        #endregion

        #region Поля расширителя

        private bool expanderOneOpen;

        private bool expanderTwoOpen;

        private bool expanderThreeOpen;

        #endregion

        #region Поля документа

        private readonly PositiveResponseModel model;

        private DateTime? contractDateUnchecked;

        private DateTime? outgoingDateUnchecked;

        private DateTime? contractTerminationDateUnchecked;

        private DateTime? statementRecalculationDateUnchecked;

        private DateTime? startDateDiscountUnchecked;

        private DateTime? endDateDiscountUnchecked;

        #endregion

        #region Поля команд

        private ICommand documentMoveCommand;

        private ICommand validateDateDiscountPanelCommand;

        private ICommand clearTextCommand;

        private ICommand expanderStatesCommand;

        private ICommand validateTextLostFocusCommand;

        private ICommand validateTextEnterButtonCommand;

        private ICommand validateTextTimersCommand;

        private ICommand updateDateChangedCommand;

        private ICommand applyDataCommand;

        private ICommand printDocumenCommandt;

        private ICommand openOptionsWindowCommand;

        private ICommand exitApplicationCommand;

        private ICommand cleanDocumentCommand;

        private ICommand openAboutWindowCommand;

        private ICommand openHelpWindowCommand;

        #endregion

        private bool wasOpenOptionsWindow;

        private MainOptionsView optionsWindow;

        private bool wasOpenAboutWindow;

        private MainAboutView aboutWindow;

        private bool wasOpenHelpWindow;

        private MainHelpView helpWindow;

        #endregion

        #region Конструкторы

        public PositiveResponseViewModel()
        {
            point = null;
            wasOpenOptionsWindow = false;
            wasOpenAboutWindow = false;
            wasOpenHelpWindow = false;
            ExpanderOneOpen = true;
            model = PositiveResponseModel.CreateNewDocument();
        }

        #endregion

        #region Свойства

        public string Title
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().
                    GetAssemblyAttributePropertyValue<System.Reflection.AssemblyTitleAttribute, string>("Title");
            }
        }

        public bool ExpanderOneOpen
        {
            get { return expanderOneOpen; }
            set
            {
                if (expanderOneOpen == value) return;
                expanderOneOpen = value;
                OnPropertyChanged(nameof(ExpanderOneOpen));
            }
        }

        public bool ExpanderTwoOpen
        {
            get { return expanderTwoOpen; }
            set
            {
                if (expanderTwoOpen == value) return;
                expanderTwoOpen = value;
                OnPropertyChanged(nameof(ExpanderTwoOpen));
            }
        }

        public bool ExpanderThreeOpen
        {
            get { return expanderThreeOpen; }
            set
            {
                if (expanderThreeOpen == value) return;
                expanderThreeOpen = value;
                OnPropertyChanged(nameof(ExpanderThreeOpen));
            }
        }

        #endregion

        #region Свойства документа

        public string ContractNumber
        {
            get { return model.ContractNumber; }
            set
            {
                if (model.ContractNumber == value) return;
                model.ContractNumber = value;
                OnPropertyChanged(nameof(ContractNumber));
            }
        }

        public DateTime? ContractDate
        {
            get { return model.ContractDate; }
            set
            {
                if (model.ContractDate == value) return;
                model.ContractDate = value;
                OnPropertyChanged(nameof(ContractDate));
            }
        }

        public string FullName
        {
            get { return model.FullName; }
            set
            {
                if (model.FullName == value) return;
                model.FullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public Gender Gender
        {
            get { return model.Gender; }
            set
            {
                if (model.Gender == value) return;
                model.Gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string Address
        {
            get { return model.Address; }
            set
            {
                if (model.Address == value) return;
                model.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string OutgoingNumber
        {
            get { return model.OutgoingNumber; }
            set
            {
                if (model.OutgoingNumber == value) return;
                model.OutgoingNumber = value;
                OnPropertyChanged(nameof(OutgoingNumber));
            }
        }

        public DateTime? OutgoingDate
        {
            get { return model.OutgoingDate; }
            set
            {
                if (model.OutgoingDate == value) return;
                model.OutgoingDate = value;
                OnPropertyChanged(nameof(OutgoingDate));
            }
        }

        public bool StatementTermination
        {
            get { return model.StatementTermination; }
            set
            {
                if (model.StatementTermination == value) return;
                model.StatementTermination = value;
                OnPropertyChanged(nameof(StatementTermination));
            }
        }

        public DateTime? ContractTerminationDate
        {
            get { return model.ContractTerminationDate; }
            set
            {
                if (model.ContractTerminationDate == value) return;
                model.ContractTerminationDate = value;
                OnPropertyChanged(nameof(ContractTerminationDate));
            }
        }

        public decimal AmountDebt
        {
            get { return model.AmountDebt; }
            set
            {
                if (model.AmountDebt == value) return;
                model.AmountDebt = value;
                OnPropertyChanged(nameof(AmountDebt));
            }
        }

        public bool StatementRecalculation
        {
            get { return model.StatementRecalculation; }
            set
            {
                if (model.StatementRecalculation == value) return;
                model.StatementRecalculation = value;
                OnPropertyChanged(nameof(StatementRecalculation));
            }
        }

        public DateTime? StatementRecalculationDate
        {
            get { return model.StatementRecalculationDate; }
            set
            {
                if (model.StatementRecalculationDate == value) return;
                model.StatementRecalculationDate = value;
                OnPropertyChanged(nameof(StatementRecalculationDate));
            }
        }

        public DiscountType DiscountType
        {
            get { return model.DiscountType; }
            set
            {
                if (model.DiscountType == value) return;
                model.DiscountType = value;
                OnPropertyChanged(nameof(DiscountType));
            }
        }

        public Discounts Discounts
        {
            get { return model.Discounts; }
            set
            {
                if (model.Discounts == value) return;
                model.Discounts = value;
                OnPropertyChanged(nameof(Discounts));
            }
        }

        public DateTime? StartDateDiscount
        {
            get { return model.StartDateDiscount; }
            set
            {
                if (model.StartDateDiscount == value) return;
                model.StartDateDiscount = value;
                OnPropertyChanged(nameof(StartDateDiscount));
            }
        }

        public DateTime? EndDateDiscount
        {
            get { return model.EndDateDiscount; }
            set
            {
                if (model.EndDateDiscount == value) return;
                model.EndDateDiscount = value;
                OnPropertyChanged(nameof(EndDateDiscount));
            }
        }

        public decimal AmountRecalculation
        {
            get { return model.AmountRecalculation; }
            set
            {
                if (model.AmountRecalculation == value) return;
                model.AmountRecalculation = value;
                OnPropertyChanged(nameof(AmountRecalculation));
            }
        }

        #endregion

        #region Свойства документа без проверки

        public DateTime? ContractDateUnchecked
        {
            get { return contractDateUnchecked; }
            set
            {
                if (contractDateUnchecked == value) return;
                contractDateUnchecked = value;
                OnPropertyChanged(nameof(ContractDateUnchecked));
            }
        }

        public DateTime? OutgoingDateUnchecked
        {
            get { return outgoingDateUnchecked; }
            set
            {
                if (outgoingDateUnchecked == value) return;
                outgoingDateUnchecked = value;
                OnPropertyChanged(nameof(OutgoingDateUnchecked));
            }
        }

        public DateTime? ContractTerminationDateUnchecked
        {
            get { return contractTerminationDateUnchecked; }
            set
            {
                if (contractTerminationDateUnchecked == value) return;
                contractTerminationDateUnchecked = value;
                OnPropertyChanged(nameof(ContractTerminationDateUnchecked));
            }
        }

        public DateTime? StatementRecalculationDateUnchecked
        {
            get { return statementRecalculationDateUnchecked; }
            set
            {
                if (statementRecalculationDateUnchecked == value) return;
                statementRecalculationDateUnchecked = value;
                OnPropertyChanged(nameof(StatementRecalculationDateUnchecked));
            }
        }

        public DateTime? StartDateDiscountUnchecked
        {
            get { return startDateDiscountUnchecked; }
            set
            {
                if (startDateDiscountUnchecked == value) return;
                startDateDiscountUnchecked = value;
                OnPropertyChanged(nameof(StartDateDiscountUnchecked));
            }
        }

        public DateTime? EndDateDiscountUnchecked
        {
            get { return endDateDiscountUnchecked; }
            set
            {
                if (endDateDiscountUnchecked == value) return;
                endDateDiscountUnchecked = value;
                OnPropertyChanged(nameof(EndDateDiscountUnchecked));
            }
        }

        #endregion

        #region Команды

        //Команда перемещение документа
        public ICommand DocumentMoveCommand
        {
            get
            {
                if (documentMoveCommand == null)
                {
                    documentMoveCommand = new RelayCommand((parameter) =>
                    {
                        ScrollViewer current = parameter as ScrollViewer;
                        if (current != null)
                        {
                            if (point == null) point = Mouse.GetPosition(current);
                            if (Mouse.LeftButton == MouseButtonState.Pressed)
                            {
                                Mouse.OverrideCursor = Cursors.Hand;
                                current.ScrollToHorizontalOffset(current.HorizontalOffset - (Mouse.GetPosition(current).X - ((Point)point).X));
                                current.ScrollToVerticalOffset(current.VerticalOffset - (Mouse.GetPosition(current).Y - ((Point)point).Y));
                                point = Mouse.GetPosition(current);
                            }
                            else if (Mouse.LeftButton == MouseButtonState.Released)
                            {
                                Mouse.OverrideCursor = null;
                                point = null;
                            }
                        }
                    });
                }
                return documentMoveCommand;
            }
            set { documentMoveCommand = value; }
        }

        //Команда: проверка даты скидок
        public ICommand ValidateDateDiscountPanelCommand
        {
            get
            {
                if (validateDateDiscountPanelCommand == null)
                {
                    validateDateDiscountPanelCommand = new RelayCommand((parameter) =>
                    {
                        StackPanel currentStackPanel = parameter as StackPanel;
                        if (currentStackPanel != null) currentStackPanel.BindingGroup.CommitEdit();
                    });
                }
                return validateDateDiscountPanelCommand;
            }
            set { validateDateDiscountPanelCommand = value; }
        }

        //Команда: Обновление привязки при изменение свойства DatePicker
        public ICommand UpdateDateChangedCommand
        {
            get
            {
                if (updateDateChangedCommand == null)
                {
                    updateDateChangedCommand = new RelayCommand((parameter) =>
                    {
                        WatermarkDatePicker currentDatePicker = parameter as WatermarkDatePicker;
                        if (currentDatePicker != null)
                        {
                            currentDatePicker.GetBindingExpression(WatermarkDatePicker.SelectedDateProperty).UpdateSource();
                        }
                    });
                }
                return updateDateChangedCommand;
            }
            set { updateDateChangedCommand = value; }
        }

        //Команда: очистить текст
        public ICommand ClearTextCommand
        {
            get
            {
                if (clearTextCommand == null)
                {
                    clearTextCommand = new RelayCommand((parameter) =>
                    {
                        WatermarkTextBox current = parameter as WatermarkTextBox;
                        if (current != null) current.Clear();
                    },
                    (parameter) =>
                    {
                        WatermarkTextBox current = parameter as WatermarkTextBox;
                        if (current != null)
                        {
                            if (!string.IsNullOrEmpty(current.Text)) return true;
                        }
                        return false;
                    });
                }
                return clearTextCommand;
            }
            set { clearTextCommand = value; }
        }

        //Команда: контроль раскрывающихся списков
        public ICommand ExpanderStatesCommand
        {
            get
            {
                if (expanderStatesCommand == null)
                {
                    expanderStatesCommand = new RelayCommand((parameter) =>
                    {
                        Expander currentExpander = parameter as Expander;
                        if (currentExpander != null)
                        {
                            switch (currentExpander.Name)
                            {
                                case "ExpanderOne":
                                    ExpanderTwoOpen = false;
                                    ExpanderThreeOpen = false;
                                    break;
                                case "ExpanderTwo":
                                    ExpanderOneOpen = false;
                                    ExpanderThreeOpen = false;
                                    break;
                                case "ExpanderThree":
                                    ExpanderOneOpen = false;
                                    ExpanderTwoOpen = false;
                                    break;
                            }
                        }
                    });
                }
                return expanderStatesCommand;
            }
            set { expanderStatesCommand = value; }
        }

        #region Проверка и коррекция текста (TextBox)

        //Команда: проверка и коррекция текста при потере фокуса
        public ICommand ValidateTextLostFocusCommand
        {
            get
            {
                if (validateTextLostFocusCommand == null)
                {
                    validateTextLostFocusCommand = new RelayCommand((parameter) =>
                    {
                        WatermarkTextBox currentTextBox = parameter as WatermarkTextBox;
                        if (currentTextBox != null)
                        {
                            if (!string.IsNullOrEmpty(currentTextBox.Text))
                            {
                                if (Regex.IsMatch(currentTextBox.Text, @"\s{2,}"))
                                {
                                    currentTextBox.Text = Regex.Replace(currentTextBox.Text, @"\s{2,}", " ").Trim();
                                    currentTextBox.CaretIndex = currentTextBox.Text.Length;
                                }
                                else if (currentTextBox.Text[0] == ' ' || currentTextBox.Text[currentTextBox.Text.Length - 1] == ' ')
                                {
                                    currentTextBox.Text = currentTextBox.Text.Trim();
                                    currentTextBox.CaretIndex = currentTextBox.Text.Length;
                                }
                            }
                            currentTextBox.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate();
                        }
                    });
                }
                return validateTextLostFocusCommand;
            }
            set { validateTextLostFocusCommand = value; }
        }

        //Команда: проверка и коррекция текста при нажатии клавиши Enter
        public ICommand ValidateTextEnterButtonCommand
        {
            get
            {
                if (validateTextEnterButtonCommand == null)
                {
                    validateTextEnterButtonCommand = new RelayCommand((parameter) =>
                    {
                        if (Keyboard.IsKeyDown(Key.Enter))
                        {
                            WatermarkTextBox currentTextBox = parameter as WatermarkTextBox;
                            if (currentTextBox != null)
                            {
                                if (!string.IsNullOrEmpty(currentTextBox.Text))
                                {
                                    if (Regex.IsMatch(currentTextBox.Text, @"\s{2,}"))
                                    {
                                        currentTextBox.Text = Regex.Replace(currentTextBox.Text, @"\s{2,}", " ").Trim();
                                        currentTextBox.CaretIndex = currentTextBox.Text.Length;
                                    }
                                    else if (currentTextBox.Text[0] == ' ' || currentTextBox.Text[currentTextBox.Text.Length - 1] == ' ')
                                    {
                                        currentTextBox.Text = currentTextBox.Text.Trim();
                                        currentTextBox.CaretIndex = currentTextBox.Text.Length;
                                    }
                                }
                                currentTextBox.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate();
                            }
                        }
                    });
                }
                return validateTextEnterButtonCommand;
            }
            set { validateTextEnterButtonCommand = value; }
        }

        //Команда: проверка и коррекция текста по таймеру
        public ICommand ValidateTextTimersCommand
        {
            get
            {
                if (validateTextTimersCommand == null)
                {
                    validateTextTimersCommand = new RelayCommand((parameter) =>
                    {
                        WatermarkTextBox currentTextBox = parameter as WatermarkTextBox;
                        if (currentTextBox != null)
                        {
                            if (timers == null) timers = new Dictionary<string, DispatcherTimer>();

                            DispatcherTimer timer;

                            if (currentTextBox.GetBindingExpression(TextBox.TextProperty).HasError)
                            {
                                Validation.ClearInvalid(currentTextBox.GetBindingExpression(TextBox.TextProperty));
                            }

                            if (timers.ContainsKey(currentTextBox.Name))
                            {
                                timer = timers[currentTextBox.Name];
                            }
                            else
                            {
                                DispatcherTimer newTimer = new DispatcherTimer();
                                newTimer.Interval = TimeSpan.FromSeconds(2.0);
                                newTimer.Tag = parameter;
                                newTimer.Tick += (sender, args) =>
                                {
                                    DispatcherTimer currentTimer = sender as DispatcherTimer;
                                    if (currentTimer != null)
                                    {
                                        WatermarkTextBox textBox = currentTimer.Tag as WatermarkTextBox;
                                        if (textBox != null)
                                        {
                                            if (!string.IsNullOrEmpty(textBox.Text))
                                            {
                                                if (Regex.IsMatch(textBox.Text, @"\s{2,}"))
                                                {
                                                    textBox.Text = Regex.Replace(textBox.Text, @"\s{2,}", " ").Trim();
                                                    textBox.CaretIndex = textBox.Text.Length;
                                                }
                                                else if (textBox.Text[0] == ' ' || textBox.Text[textBox.Text.Length - 1] == ' ')
                                                {
                                                    textBox.Text = textBox.Text.Trim();
                                                    textBox.CaretIndex = textBox.Text.Length;
                                                }
                                            }
                                            textBox.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate();
                                        }
                                        currentTimer.Stop();
                                    }
                                };
                                timer = newTimer;
                                timers.Add(currentTextBox.Name, newTimer);
                            }
                            timer.Stop();
                            timer.Start();
                        }
                    });
                }
                return validateTextTimersCommand;
            }
            set { validateTextTimersCommand = value; }
        }

        #endregion

        //Команда: применение введенных данных
        public ICommand ApplyDataCommand
        {
            get
            {
                if (applyDataCommand == null)
                {
                    applyDataCommand = new RelayCommand((parameter) =>
                    {
                        List<FrameworkElement> elements = parameter as List<FrameworkElement>;
                        if (elements != null)
                        {
                            foreach (FrameworkElement element in elements)
                            {
                                if (element.Name == "ContractDate" || element.Name == "OutgoingDate" ||
                                    element.Name == "ContractTerminationDate" || element.Name == "StatementRecalculationDate" ||
                                    element.Name == "DateDiscountPanel" || element.Name == "StartDateDiscount" ||
                                    element.Name == "EndDateDiscounts") continue;
                                else if (element is WatermarkTextBox)
                                {
                                    element.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                                }
                            }
                            if (ContractDate != ContractDateUnchecked) ContractDate = ContractDateUnchecked;
                            if (OutgoingDate != OutgoingDateUnchecked) OutgoingDate = OutgoingDateUnchecked;
                            if (ContractTerminationDate != ContractTerminationDateUnchecked) ContractTerminationDate = ContractTerminationDateUnchecked;
                            if (StatementRecalculationDate != StatementRecalculationDateUnchecked) StatementRecalculationDate = StatementRecalculationDateUnchecked;
                            if (StartDateDiscount != StartDateDiscountUnchecked) StartDateDiscount = StartDateDiscountUnchecked;
                            if (EndDateDiscount != EndDateDiscountUnchecked) EndDateDiscount = EndDateDiscountUnchecked;
                        }
                    }, (parameter) =>
                    {
                        List<FrameworkElement> elements = parameter as List<FrameworkElement>;
                        if (elements != null)
                        {
                            foreach (FrameworkElement element in elements)
                            {

                                #region Пропуск не проверяемых элементов

                                if (StatementTermination)
                                {
                                    if (element.Name == "DateDiscountPanel" || element.Name == "StartDateDiscount" || element.Name == "EndDateDiscounts") continue;
                                }
                                else
                                {
                                    switch(DiscountType)
                                    {
                                        case DiscountType.Discount:
                                            if (element.Name == "ContractTerminationDate" || element.Name == "AmountDebt" || element.Name == "AmountRecalculation") continue;
                                            break;
                                        case DiscountType.Recalculation:
                                            if (element.Name == "ContractTerminationDate" || element.Name == "AmountDebt" ||
                                                element.Name == "DateDiscountPanel" || element.Name == "StartDateDiscount" ||
                                                element.Name == "EndDateDiscounts") continue;
                                            break;
                                        case DiscountType.DiscountAndRecalculation:
                                            if (element.Name == "ContractTerminationDate" || element.Name == "AmountDebt") continue;
                                            break;
                                    }
                                }

                                #endregion

                                #region Проверка элементов

                                if (element is WatermarkTextBox)
                                {
                                    if (element.GetBindingExpression(TextBox.TextProperty).HasError) return false;
                                    else if (element.Name != "AmountDebt")
                                    {
                                        if (string.IsNullOrEmpty(((WatermarkTextBox)element).Text)) return false;
                                    }
                                }
                                if (element is WatermarkDatePicker)
                                {
                                    if (element.GetBindingExpression(WatermarkDatePicker.SelectedDateProperty).HasError) return false;
                                    else if (((WatermarkDatePicker)element).SelectedDate == null) return false;
                                }
                                if (element is StackPanel)
                                {
                                    if (element.BindingGroup.HasValidationError) return false;
                                }

                                #endregion

                            }
                        }
                        return true;
                    });
                }
                return applyDataCommand;
            }
            set { applyDataCommand = value; }
        }

        //Команда: печать документа
        public ICommand PrintDocumentCommand
        {
            get
            {
                if (printDocumenCommandt == null)
                {
                    printDocumenCommandt = new RelayCommand((parameter) =>
                    {
                        PrintDialog printDialog = new PrintDialog();
                        if (printDialog.ShowDialog() == true)
                        {
                            ContentControl document = parameter as ContentControl;
                            if (document != null)
                            {
                                Transform transform = document.LayoutTransform;
                                document.LayoutTransform = new ScaleTransform(1.0, 1.0);
                                double pageMargin = 0.0;
                                Size pageSize = new Size(printDialog.PrintableAreaWidth - pageMargin * 2.0, printDialog.PrintableAreaHeight - pageMargin * 2.0);
                                document.Measure(pageSize);
                                document.Arrange(new Rect(pageMargin, pageMargin, pageSize.Width, pageSize.Height));
                                printDialog.PrintVisual(document, Properties.Resources.PositiveResponse);
                                document.LayoutTransform = transform;
                            }
                        }
                    });
                }
                return printDocumenCommandt;
            }
            set { printDocumenCommandt = value; }
        }

        //Команда: открыть окно настройки программы
        public ICommand OpenOptionsWindowCommand
        {
            get
            {
                if (openOptionsWindowCommand == null)
                {
                    openOptionsWindowCommand = new RelayCommand((parameter) =>
                    {
                        if (optionsWindow == null)
                        {
                            optionsWindow = new MainOptionsView()
                            {
                                ResizeMode = ResizeMode.NoResize,
                                ShowInTaskbar = false,
                                Owner = Application.Current.MainWindow,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            };
                            optionsWindow.Activated += (sender, args) => wasOpenOptionsWindow = true;
                            optionsWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                            optionsWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenOptionsWindow);
                        }
                        optionsWindow.ShowDialog();
                    });
                }
                return openOptionsWindowCommand;
            }
            set { openOptionsWindowCommand = value; }
        }

        //Команда: выход из программы
        public ICommand ExitApplicationCommand
        {
            get
            {
                if (exitApplicationCommand == null)
                {
                    exitApplicationCommand = new RelayCommand((parameter) => Application.Current.Shutdown());
                }
                return exitApplicationCommand;
            }
            set { exitApplicationCommand = value; }
        }

        //Команда: очистить документ cleanDocumentCommand
        public ICommand CleanDocumentCommand
        {
            get
            {
                if (cleanDocumentCommand == null)
                {
                    cleanDocumentCommand = new RelayCommand((parameter) =>
                    {
                        List<FrameworkElement> elements = parameter as List<FrameworkElement>;
                        if (elements != null)
                        {
                            foreach (FrameworkElement element in elements)
                            {
                                if (element is Expander) ((Expander)element).IsExpanded = true;
                                else if (element is WatermarkTextBox)
                                {
                                    ((WatermarkTextBox)element).Clear();
                                    element.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                                }
                                else if (element is WatermarkDatePicker) ((WatermarkDatePicker)element).Clear();
                                else if (element is RadioButton) ((RadioButton)element).IsChecked = true;
                                else if (element is CheckBox) ((CheckBox)element).IsChecked = false;
                                else if (element is ComboBox) ((ComboBox)element).SelectedIndex = 0;
                            }
                            if (ContractDate != ContractDateUnchecked) ContractDate = ContractDateUnchecked;
                            if (OutgoingDate != OutgoingDateUnchecked) OutgoingDate = OutgoingDateUnchecked;
                            if (ContractTerminationDate != ContractTerminationDateUnchecked) ContractTerminationDate = ContractTerminationDateUnchecked;
                            if (StatementRecalculationDate != StatementRecalculationDateUnchecked) StatementRecalculationDate = StatementRecalculationDateUnchecked;
                            if (StartDateDiscount != StartDateDiscountUnchecked) StartDateDiscount = StartDateDiscountUnchecked;
                            if (EndDateDiscount != EndDateDiscountUnchecked) EndDateDiscount = EndDateDiscountUnchecked;
                        }
                    });
                }
                return cleanDocumentCommand;
            }
            set { cleanDocumentCommand = value; }
        }

        //Команда: открыть окно о программе
        public ICommand OpenAboutWindowCommand
        {
            get
            {
                if (openAboutWindowCommand == null)
                {
                    openAboutWindowCommand = new RelayCommand((parameter) =>
                    {
                        if (aboutWindow == null)
                        {
                            aboutWindow = new MainAboutView()
                            {
                                ResizeMode = ResizeMode.NoResize,
                                ShowInTaskbar = false,
                                Owner = Application.Current.MainWindow,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            };
                            aboutWindow.Activated += (sender, args) => wasOpenAboutWindow = true;
                            aboutWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                            aboutWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenAboutWindow);
                        }
                        aboutWindow.ShowDialog();
                    });
                }
                return openAboutWindowCommand;
            }
            set { openAboutWindowCommand = value; }
        }

        //Команда: открыть окно справки
        public ICommand OpenHelpWindowCommand
        {
            get
            {
                if (openHelpWindowCommand == null)
                {
                    openHelpWindowCommand = new RelayCommand((parameter) =>
                    {
                        if (helpWindow == null)
                        {
                            helpWindow = new MainHelpView()
                            {
                                ResizeMode = ResizeMode.CanMinimize,
                                Owner = Application.Current.MainWindow,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            };
                            helpWindow.Activated += (sender, args) => wasOpenHelpWindow = true;
                            helpWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                            helpWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenHelpWindow);
                        }
                        helpWindow.Show();
                    });
                }
                return openHelpWindowCommand;
            }
            set { openHelpWindowCommand = value; }
        }

        #endregion

        #region Реализация интерфейса IDataErrorInfo

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;

                #region Проверка даты ответа в зависимости от даты договора

                if (columnName == nameof(OutgoingDateUnchecked))
                {
                    if (ContractDateUnchecked != null && OutgoingDateUnchecked != null)
                    {
                        if (((DateTime)OutgoingDateUnchecked - (DateTime)ContractDateUnchecked).Days < 0)
                        {
                            result = Properties.Resources.OutgoingDateNotValid;
                        }
                    }
                    else if (ContractDateUnchecked == null && OutgoingDateUnchecked != null)
                    {
                        result = Properties.Resources.ContractDateNotValid;
                    }
                }

                #endregion

                #region Проверка даты расторжения в зависимости от даты договора

                if (columnName == nameof(ContractTerminationDateUnchecked))
                {
                    if (ContractDateUnchecked != null && ContractTerminationDateUnchecked != null)
                    {
                        if (((DateTime)ContractTerminationDateUnchecked - (DateTime)ContractDateUnchecked).Days < 0)
                        {
                            result = Properties.Resources.ContractTerminationDateNotValid;
                        }
                    }
                    else if (ContractDateUnchecked == null && ContractTerminationDateUnchecked != null)
                    {
                        result = Properties.Resources.ContractDateNotValid;
                    }
                }

                #endregion

                #region Проверка даты заявления на перерасчет в зависимости от даты договора

                if (columnName == nameof(StatementRecalculationDateUnchecked))
                {
                    if (ContractDateUnchecked != null && StatementRecalculationDateUnchecked != null)
                    {
                        if (((DateTime)StatementRecalculationDateUnchecked - (DateTime)ContractDateUnchecked).Days < 0)
                        {
                            result = Properties.Resources.StatementRecalculationDateNotValid;
                        }
                    }
                    else if (ContractDateUnchecked == null && StatementRecalculationDateUnchecked != null)
                    {
                        result = Properties.Resources.ContractDateNotValid;
                    }
                }

                #endregion

                return result;
            }
        }

        #endregion

        #region Методы

        private void CancelClosingWindow(Window window, CancelEventArgs arguments)
        {
            if (window == null) return;
            window.Visibility = Visibility.Hidden;
            arguments.Cancel = true;
        }

        private void CenteringWindow(Window window, bool wasOpen)
        {
            if (window == null) return;
            if (window.Owner == null) return;
            if (window.IsVisible & wasOpen)
            {
                if (window.Owner.WindowState == WindowState.Normal)
                {
                    window.Left = window.Owner.Left + (window.Owner.ActualWidth - window.ActualWidth) / 2.0;
                    window.Top = window.Owner.Top + (window.Owner.ActualHeight - window.ActualHeight) / 2.0;
                }
                else if (window.Owner.WindowState == WindowState.Maximized)
                {
                    window.Left = (SystemParameters.PrimaryScreenWidth - window.Width) / 2.0;
                    window.Top = (SystemParameters.PrimaryScreenHeight - window.Height) / 2.0;
                }
            }
        }

        #endregion

    }
}