using Bachup.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model
{
    class Settings : INotifyPropertyChanged
    {
        public Settings()
        {
            DarkMode = false;
            KeepOnTop = false;
            Color = ThemeColors.red;
        }

        public void ResetSettings()
        {
            DarkMode = false;
            Color = ThemeColors.red;
            KeepOnTop = false;
        }

        // Basic ViewModelBase
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _darkMode;
        public  bool DarkMode
        {
            get { return _darkMode; }
            set
            {
                _darkMode = value;
                NotifyPropertyChanged();
            }
        }

        private  ThemeColors _color;
        public  ThemeColors Color
        {
            get { return _color;  }
            set
            {
                _color = value;
                NotifyPropertyChanged();
            }
        }

        private bool _keepOnTop;
        public bool KeepOnTop
        {
            get { return _keepOnTop; }
            set
            {
                _keepOnTop = value;
                NotifyPropertyChanged();
            }
        }

       
    }
}
