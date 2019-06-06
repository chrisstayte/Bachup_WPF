using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model
{
    class BachupHistory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BachupHistory()
        {
            BachupDestinationStatus = new Dictionary<string, bool>();
        }

        private DateTime _bachupDateTime;
        public DateTime BachupDateTime
        {
            get { return _bachupDateTime; }
            set
            {
                if (_bachupDateTime != value)
                {
                    _bachupDateTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Dictionary<String, bool> _bachupDestinationStatus;
        public Dictionary<String, bool> BachupDestinationStatus;

        public string GetStatusMessage()
        {
            int good = 0;
            int bad = 0;

            foreach (var status in BachupDestinationStatus.Values)
            {
                good = status ? good += 1 : good;
                bad = status ? bad : bad += 1;
            }

            return good == BachupDestinationStatus.Count ? "Success Fule All Backed Up" : "FAILED";
        }

    }
}
