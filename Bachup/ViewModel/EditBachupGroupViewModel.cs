using Bachup.Helpers;
using Bachup.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class EditBachupGroupViewModel : BaseViewModel
    {
        public EditBachupGroupViewModel(BachupGroup bg)
        {
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);

            ShowMessage = false;
            Message = "";
            _backupGroup = bg;
            Name = bg.Name;
        }

        private readonly BachupGroup _backupGroup;

        private string _name;
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _message;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get { return _showMessage; }
            set
            {
                _showMessage = value;
                NotifyPropertyChanged();
            }
        }



        // Relay Commands

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

       

        #region Events

        private void Cancel(object paraemeter)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void Save(object parameter)
        {
            if (CheckRequirements())
            {
                _backupGroup.Name = Name;
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            
        }

        private bool CheckRequirements()
        {
            if (_name == null)
            {
                Message = "Enter a Name";
                ShowMessage = true;
                return false;
            }

            ShowMessage = false;
            return true;
        }

        #endregion
    }
}
