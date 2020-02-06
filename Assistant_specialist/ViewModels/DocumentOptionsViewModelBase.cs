using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant_specialist.ViewModels
{
    public abstract class DocumentOptionsViewModelBase : ViewModelBase
    {

        #region Поля

        private string nameDocument;

        #endregion

        #region Свойства

        public string NameDocument
        {
            set
            {
                if (nameDocument == value) return;
                nameDocument = value;
                OnPropertyChanged(nameof(NameDocument));
            }
            get { return nameDocument; }
        }

        #endregion

        #region Методы

        public abstract void Save();

        public abstract void Cancel();

        public abstract void Reset();

        #endregion

    }
}