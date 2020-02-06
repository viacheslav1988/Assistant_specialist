using Assistant_specialist.ViewModels.PositiveResponseVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Assistant_specialist.ViewModels
{
    class MainOptionsViewModel : ViewModelBase
    {

        #region Поля

        private ObservableCollection<DocumentOptionsViewModelBase> documentOptions;

        private bool isSaved;

        private ICommand closingCommand;

        private ICommand okCommand;

        private ICommand cancelCommand;

        private ICommand previousCommand;

        private ICommand nextCommand;

        private ICommand resetCommand;

        #endregion

        #region Свойства

        public ObservableCollection<DocumentOptionsViewModelBase> DocumentOptions
        {
            get
            {
                if (documentOptions == null)
                {
                    documentOptions = new ObservableCollection<DocumentOptionsViewModelBase>()
                    {
                        new PositiveResponseOptionsViewModel()
                    };
                }
                return documentOptions;
            }
            set { documentOptions = value; }
        }

        #endregion

        #region Команды

        //Команда: закрыть окно
        public ICommand ClosingCommand
        {
            get
            {
                if (closingCommand == null)
                {
                    closingCommand = new RelayCommand((parameter) =>
                    {
                        if (documentOptions != null)
                        {
                            if (!isSaved)
                            {
                                foreach (DocumentOptionsViewModelBase options in DocumentOptions)
                                {
                                    options.Cancel();
                                }
                            }
                            isSaved = false;
                        }
                    });
                }
                return closingCommand;
            }
            set { closingCommand = value; }
        }

        //Команда: ок
        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand((parameter) =>
                    {
                        if (documentOptions != null)
                        {
                            isSaved = true;
                            foreach (DocumentOptionsViewModelBase options in DocumentOptions)
                            {
                                options.Save();
                            }
                        }
                        Window optionsWindow = parameter as Window;
                        if (optionsWindow != null)
                        {
                            optionsWindow.Close();
                        }
                    });
                }
                return okCommand;
            }
            set { okCommand = value; }
        }

        //Команда: отмена
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand((parameter) =>
                    {
                        Window optionsWindow = parameter as Window;
                        if (optionsWindow != null)
                        {
                            optionsWindow.Close();
                        }
                    });
                }
                return cancelCommand;
            }
            set { cancelCommand = value; }
        }

        //Команда: назад
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand((parameter) =>
                    {
                        ICollectionView view = CollectionViewSource.GetDefaultView(documentOptions);
                        if (view != null)
                        {
                            view.MoveCurrentToPrevious();
                            if (view.IsCurrentBeforeFirst) view.MoveCurrentToLast();
                        }
                    });
                }
                return previousCommand;
            }
            set { previousCommand = value; }
        }

        //Команда: вперед
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand((parameter) =>
                    {
                        ICollectionView view = CollectionViewSource.GetDefaultView(documentOptions);
                        if (view != null)
                        {
                            view.MoveCurrentToNext();
                            if (view.IsCurrentAfterLast) view.MoveCurrentToFirst();
                        }
                    });
                }
                return nextCommand;
            }
            set { nextCommand = value; }
        }

        //Команда: сброс
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand((parameter) =>
                    {
                        ICollectionView view = CollectionViewSource.GetDefaultView(documentOptions);
                        if (view != null)
                        {
                            DocumentOptionsViewModelBase currentOptions = view.CurrentItem as DocumentOptionsViewModelBase;
                            if (currentOptions != null)
                            {
                                currentOptions.Reset();
                            }
                        }
                    });
                }
                return resetCommand;
            }
            set { resetCommand = value; }
        }

        #endregion

    }
}