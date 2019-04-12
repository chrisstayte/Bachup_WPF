using Bachup.Helpers;
using Bachup.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;

namespace Bachup.ViewModel
{
    class AddDestinationViewModel : BaseViewModel
    {
        public AddDestinationViewModel(BachupItem bachupItem)
        {
            CancelCommand = new RelayCommand(Cancel);
            AddCommand = new RelayCommand(Add);
            AddDestinationCommand = new RelayCommand(AddDestination);

            _bachupItem = bachupItem;
        }

        // Properties
        private readonly BachupItem _bachupItem;

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

        private string _destination;
        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
                NotifyPropertyChanged();
            }
        }


        // Relay Commands
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand AddDestinationCommand { get; private set; }

        #region Events

        private void Cancel(object o)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void Add(object o)
        {
            if (_bachupItem.IsDestinationADuplicate(Destination))
            {
                Message = "Destination Alreadys Exists";
                ShowMessage = true;
                return;
            }

            if (!Directory.Exists(Destination))
            {
                Message = "Destination Path Does Not Exist";
                ShowMessage = true;

                return;
            }

            ShowMessage = false;
            _bachupItem.AddDestination(Destination);
            MainViewModel.SaveData();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        #endregion


        #region Methods



        private void AddDestination(object o)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (Directory.Exists(Destination))
            {
                dialog.InitialDirectory = Destination;
            }

            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Destination = dialog.FileName;                
            }

            ShowMessage = false;
        }

        #endregion
    }
}
