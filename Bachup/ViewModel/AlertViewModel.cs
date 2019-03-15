using Bachup.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class AlertViewModel : INotifyPropertyChanged
    {
        public AlertViewModel(String message)
        {
            Message = message;
        }


        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                this.MutateVerbose(ref _message, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

    }
}
