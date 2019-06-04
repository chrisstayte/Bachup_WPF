using Bachup.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bachup.Model
{
    class Settings : INotifyPropertyChanged
    {
        public Settings()
        {
            DarkMode = false;
            KeepOnTop = false;
            Color = ThemeColors.red;
            LastOpened = null;

        }

        public void ResetSettings()
        {
            DarkMode = false;
            Color = ThemeColors.red;
            KeepOnTop = false;
            LastOpened = null;
            Beta = false;
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

        private bool _openToLastSelected;
        public bool OpenToLastSelected
        {
            get
            {
                return _openToLastSelected;
            }
            set
            {
                _openToLastSelected = value;
                NotifyPropertyChanged();
            }
        }

        private Nullable<Guid> _lastOpened;
        public Nullable<Guid> LastOpened
        {
            get { return _lastOpened; }
            set
            {
                _lastOpened = value;
                NotifyPropertyChanged();
            }
        }

        private bool _beta;
        public bool Beta
        {
            get
            {
                return _beta;
            }
            set
            {
                if (_beta != value)
                {
                    _beta = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #region Methods

        public void ExpandAll(bool expand)
        {
            foreach (BachupGroup bg in Bachup.ViewModel.MainViewModel.Bachup)
            {
                bg.IsExpanded = expand;
            }
        }

        public void DeselectAll()
        {
            foreach (BachupGroup bg in Bachup.ViewModel.MainViewModel.Bachup)
            {
                bg.IsSelected = false;
                
                foreach (BachupItem bi in bg.BachupItems)
                {
                    bi.IsSelected = false;
                }
            }
        }

        public void SetTheme()
        {
            try
            {
                new PaletteHelper().ReplaceAccentColor(MainViewModel.Settings.Color.ToString());
                new PaletteHelper().ReplacePrimaryColor(MainViewModel.Settings.Color.ToString());

            }
            catch
            {
                MainViewModel.Settings.ResetSettings();
                new PaletteHelper().ReplaceAccentColor(MainViewModel.Settings.Color.ToString());
                new PaletteHelper().ReplacePrimaryColor(MainViewModel.Settings.Color.ToString());


            }
        }

        public void SetDarkMode()
        {
            try
            {
                new PaletteHelper().SetLightDark(DarkMode);
            }
            catch
            {
                ResetSettings();
                new PaletteHelper().SetLightDark(DarkMode);
            }
        }
        #endregion

    }
}
