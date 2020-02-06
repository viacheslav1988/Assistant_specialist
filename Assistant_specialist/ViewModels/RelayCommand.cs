using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Assistant_specialist.ViewModels
{
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            execute(parameter);
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }
    }
}