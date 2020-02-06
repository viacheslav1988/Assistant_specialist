using Assistant_specialist.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Assistant_specialist.ViewModels
{
    class MainAboutViewModel : ViewModelBase
    {

        #region Поля

        private Assembly assembly;

        private ICommand closeCurrentWindowCommand;

        #endregion

        #region Конструкторы

        public MainAboutViewModel()
        {
            assembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        #region Свойства

        public string Title
        {
            get { return assembly.GetAssemblyAttributePropertyValue<AssemblyTitleAttribute, string>("Title"); }
        }

        public string Version
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(assembly.GetName().Version.Major).Append('.');
                if (assembly.GetName().Version.Minor == 0) result.Append('0');
                else if (assembly.GetName().Version.Minor % 10 == 0) result.Append(assembly.GetName().Version.Minor / 10);
                else result.AppendFormat("{0:00}", assembly.GetName().Version.Minor);
                return result.ToString();
            }
        }

        public string Copyright
        {
            get { return assembly.GetAssemblyAttributePropertyValue<AssemblyCopyrightAttribute, string>("Copyright"); }
        }

        #endregion

        #region Команды

        //Команда: закрыть текущее окно
        public ICommand CloseCurrentWindowCommand
        {
            get
            {
                if (closeCurrentWindowCommand == null)
                {
                    closeCurrentWindowCommand = new RelayCommand((parameter) =>
                    {
                        Window current = parameter as Window;
                        if (current != null) current.Close();
                    });
                }
                return closeCurrentWindowCommand;
            }
            set { closeCurrentWindowCommand = value; }
        }

        #endregion

    }
}