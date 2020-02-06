using Assistant_specialist.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Assistant_specialist.ViewModels.PositiveResponseVM
{
    class PositiveResponseOptionsViewModel : DocumentOptionsViewModelBase
    {

        #region Поля

        private Dictionary<string, DispatcherTimer> timers;

        private ICommand clearTextCommand;

        private ICommand updateTextLostFocusCommand;

        private ICommand updateTextEnterButtonCommand;

        private ICommand updateTextTimersCommand;

        #endregion

        #region Конструкторы

        public PositiveResponseOptionsViewModel()
        {
            NameDocument = "Положительный ответ";
            Properties.PositiveResponseSettings.Default.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }

        #endregion

        #region Свойства

        //Полный почтовый адрес
        public bool FullPostalAddress
        {
            get { return Properties.PositiveResponseSettings.Default.FullPostalAddress; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.FullPostalAddress == value) return;
                Properties.PositiveResponseSettings.Default.FullPostalAddress = value;
            }
        }

        //Склонение имени
        public bool DeclensionFullName
        {
            get { return Properties.PositiveResponseSettings.Default.DeclensionFullName; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.DeclensionFullName == value) return;
                Properties.PositiveResponseSettings.Default.DeclensionFullName = value;
            }
        }

        //Должность подписывающего
        public string PositionSignDocuments
        {
            get { return Properties.PositiveResponseSettings.Default.PositionSignDocuments; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.PositionSignDocuments == value) return;
                Properties.PositiveResponseSettings.Default.PositionSignDocuments = value;
            }
        }

        //Ф.И.О. подписывающего
        public string NameSignDocuments
        {
            get { return Properties.PositiveResponseSettings.Default.NameSignDocuments; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.NameSignDocuments == value) return;
                Properties.PositiveResponseSettings.Default.NameSignDocuments = value;
            }
        }

        //Показывать имя специалиста
        public bool ShowSpecialist
        {
            get { return Properties.PositiveResponseSettings.Default.ShowSpecialist; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.ShowSpecialist == value) return;
                Properties.PositiveResponseSettings.Default.ShowSpecialist = value;
            }
        }

        //Имя специалиста
        public string NameSpecialist
        {
            get { return Properties.PositiveResponseSettings.Default.NameSpecialist; }
            set
            {
                if (Properties.PositiveResponseSettings.Default.NameSpecialist == value) return;
                Properties.PositiveResponseSettings.Default.NameSpecialist = value;
            }
        }

        #endregion

        #region Команды

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

        //Команда: коррекция и обновление текста при потере фокуса
        public ICommand UpdateTextLostFocusCommand
        {
            get
            {
                if (updateTextLostFocusCommand == null)
                {
                    updateTextLostFocusCommand = new RelayCommand((parameter) =>
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
                            currentTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        }
                    });
                }
                return updateTextLostFocusCommand;
            }
            set { updateTextLostFocusCommand = value; }
        }

        //Команда: коррекция и обновление текста при нажатии клавиши Enter
        public ICommand UpdateTextEnterButtonCommand
        {
            get
            {
                if (updateTextEnterButtonCommand == null)
                {
                    updateTextEnterButtonCommand = new RelayCommand((parameter) =>
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
                                currentTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                            }
                        }
                    });
                }
                return updateTextEnterButtonCommand;
            }
            set { updateTextEnterButtonCommand = value; }
        }

        //Команда: коррекция и обновление текста текста по таймеру
        public ICommand UpdateTextTimersCommand
        {
            get
            {
                if (updateTextTimersCommand == null)
                {
                    updateTextTimersCommand = new RelayCommand((parameter) =>
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
                                            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
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
                return updateTextTimersCommand;
            }
            set { updateTextTimersCommand = value; }
        }

        #endregion

        #region Методы

        public override void Save() { Properties.PositiveResponseSettings.Default.Save(); }

        public override void Cancel() { Properties.PositiveResponseSettings.Default.Reload(); }

        public override void Reset() { Properties.PositiveResponseSettings.Default.Reset(); }

        #endregion

    }
}