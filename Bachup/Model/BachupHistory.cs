using Bachup.Model;
using MaterialDesignThemes.Wpf;
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

        private BachupHistoryType _type;
        public BachupHistoryType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged();
            }
        }

        private PackIconKind _icon;
        public PackIconKind Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void GetStatus()
        {
            int good = 0;
            int bad = 0;
            int total = BachupDestinationStatus.Count;

            foreach (bool status in BachupDestinationStatus.Values)
            {
                good = status ? good += 1 : good;
                bad = status ? bad : bad += 1;
            }

            if (good == total)
            {
                Icon = PackIconKind.Check;
                Type = BachupHistoryType.fullBachup;
                return;
            }

            if (bad == total)
            {
                Icon = PackIconKind.CloseOctagonOutline;
                Type = BachupHistoryType.failedBachup;
                return;
            }
            Icon = PackIconKind.WarningOutline;
            Type = BachupHistoryType.partialBachup;
        }

    }
}
