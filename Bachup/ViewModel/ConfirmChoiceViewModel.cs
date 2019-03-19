using Bachup.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class ConfirmChoiceViewModel : BaseViewModel
    {
        public ConfirmChoiceViewModel(string message)
        {
            SetupCommands();
            Message = message;
        }

        public ConfirmChoiceViewModel(string message, string submessage)
        {
            SetupCommands();
            Message = message;
            SubMessage = submessage;
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyPropertyChanged();
            }
        }

        private string _subMessage;
        public string SubMessage
        {
            get { return _subMessage; }
            set
            {
                _subMessage = value;
                NotifyPropertyChanged();
            }
        }

        // Relay Commands
        public RelayCommand NoCommand { get; private set; }
        public RelayCommand YesCommand { get; private set; }

        #region Events

        private void No(object o)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void Yes(object o)
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        #endregion

        #region Methods

        private void SetupCommands()
        {
            NoCommand = new RelayCommand(No);
            YesCommand = new RelayCommand(Yes);
        }

        #endregion

    }
}
