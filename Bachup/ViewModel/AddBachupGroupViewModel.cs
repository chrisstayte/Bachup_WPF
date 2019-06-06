using Bachup.Helpers;
using Bachup.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;

namespace Bachup.ViewModel
{
    class AddBachupGroupViewModel : BaseViewModel
    {
        public AddBachupGroupViewModel()
        {
            CancelCommand = new RelayCommand(Cancel);
            AddCommand = new RelayCommand(Add);

            ShowMessage = false;
            Message = "";
        }

        private string _name;
        public string Name
        {
            get { return _name; }
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
        public RelayCommand AddCommand { get; private set; }

        #region Events

        private void Cancel(object parameter)
        {
            Debug.WriteLine("CANCEL BUTTON CLICKED");
            DialogHost.CloseDialogCommand.Execute(null, null);
            
        }

        private void Add(object parameter)
        {
            if (CheckRequirements())
            {
                BachupGroup bg = new BachupGroup(Name);

                DialogHost.CloseDialogCommand.Execute(bg, null);
            }
        }

        #endregion

        #region Methods

        private bool CheckRequirements()
        {
            if (_name == null)
            {
                Message = "Enter a Name";
                ShowMessage = true;
                return false;
            }

            if (MainViewModel.DoesBachupGroupExist(_name))
            {
                Message = "Group With That Name Exists";
                ShowMessage = true;
                return false;
            }

            ShowMessage = false;
            return true;
        }

        #endregion

    }
}
