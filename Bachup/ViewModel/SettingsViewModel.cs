using Bachup.Helpers;
using Bachup.Model;
using System;

namespace Bachup.ViewModel
{
    class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            LinkCommand();
            SetColorThemeStatus();
        }

        #region Color Properties

        private bool _yellowActive;
        public bool YellowActive
        {
            get { return _yellowActive; }
            set
            {
                _yellowActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _amberActive;
        public bool AmberActive
        {
            get { return _amberActive; }
            set
            {
                _amberActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _deepOrangeActive;
        public bool DeepOrangeActive
        {
            get { return _deepOrangeActive; }
            set
            {
                _deepOrangeActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _lightBlueActive;
        public bool LightBlueActive
        {
            get { return _lightBlueActive; }
            set
            {
                _lightBlueActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _tealActive;
        public bool TealActive
        {
            get { return _tealActive; }
            set
            {
                _tealActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _cyanActive;
        public bool CyanActive
        {
            get { return _cyanActive; }
            set
            {
                _cyanActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _pinkActive;
        public bool PinkActive
        {
            get { return _pinkActive; }
            set
            {
                _pinkActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _greenActive;
        public bool GreenActive
        {
            get { return _greenActive; }
            set
            {
                _greenActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _deepPurpleActive;
        public bool DeepPurpleActive
        {
            get { return _deepPurpleActive; }
            set
            {
                _deepPurpleActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _indigoActive;
        public bool IndigoActive
        {
            get { return _indigoActive; }
            set
            {
                _indigoActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _lightGreenActive;
        public bool LightGreenActive
        {
            get { return _lightGreenActive; }
            set
            {
                _lightGreenActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _blueActive;
        public bool BlueActive
        {
            get { return _blueActive; }
            set
            {
                _blueActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _limeActive;
        public bool LimeActive
        {
            get { return _limeActive; }
            set
            {
                _limeActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _redActive;
        public bool RedActive
        {
            get { return _redActive; }
            set
            {
                _redActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _orangeActive;
        public bool OrangeActive
        {
            get { return _orangeActive; }
            set
            {
                _orangeActive = value;
                NotifyPropertyChanged();
            }
        }

        private bool _purpleActive;
        public bool PurpleActive
        {
            get { return _purpleActive; }
            set
            {
                _purpleActive = value;
                NotifyPropertyChanged();
            }
        }


        private string _themeName;
        public string ThemeName
        {
            get
            {
                return _themeName;
            }
            set
            {
                if (_themeName != value)
                {
                    _themeName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool DarkMode
        {
            get { return MainViewModel.Settings.DarkMode; }
            set
            {
                MainViewModel.Settings.DarkMode = value;
                NotifyPropertyChanged();
            }
        }

        public bool KeepOnTop
        {
            get { return MainViewModel.Settings.KeepOnTop; }
            set
            {
                MainViewModel.Settings.KeepOnTop = value;
                NotifyPropertyChanged();
            }
        }

        public bool SaveView
        {
            get { return MainViewModel.Settings.OpenToLastSelected; }
            set
            {
                MainViewModel.Settings.OpenToLastSelected = value;
                NotifyPropertyChanged();
            }
        }

        public bool Beta
        {
            get { return MainViewModel.Settings.Beta; }
            set
            {
                MainViewModel.Settings.Beta = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowNotifications
        {
            get { return MainViewModel.Settings.ShowNotifications; }
            set
            {
                MainViewModel.Settings.ShowNotifications = value;
                NotifyPropertyChanged();
            }
        }

        public int CompressionLevel
        {
            get
            {
                return (int)MainViewModel.Settings.CompressionLevel;
            }
            set
            {
                MainViewModel.Settings.CompressionLevel = (CompressionLevel) value;
                NotifyPropertyChanged();
                MainViewModel.SaveSettings();
            }
        }

        // RelayCommands
        public RelayCommand SetThemeCommand { get; private set; }
        public RelayCommand SetDarkModeCommand { get; private set; }
        public RelayCommand SaveSettingsCommand { get; private set; }
        public RelayCommand ShowSiteCommand { get; private set; }

        #endregion

        #region Events

        private void SetTheme(object o)
        {

            MainViewModel.Settings.Color = (ThemeColors)o;

            MainViewModel.Settings.SetTheme();

            SetColorThemeStatus();

            MainViewModel.SaveSettings();
        }

        private void SetDarkMode(object o)
        {
            MainViewModel.Settings.SetDarkMode();

            SetColorThemeStatus();

            MainViewModel.SaveSettings();
        }

        private void ShowSite(object o)
        {
            System.Diagnostics.Process.Start("http://chrisstayte.com");
        }

        private void SaveSettings(object o)
        {
            MainViewModel.SaveSettings();
        }

        #endregion

        #region Methods

        private void LinkCommand()
        {
            SetThemeCommand = new RelayCommand(SetTheme);
            SetDarkModeCommand = new RelayCommand(SetDarkMode);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ShowSiteCommand = new RelayCommand(ShowSite);
        }

        

        private void SetColorThemeStatus()
        {
            YellowActive = MainViewModel.Settings.Color == ThemeColors.yellow;
            AmberActive = MainViewModel.Settings.Color == ThemeColors.amber;
            DeepOrangeActive = MainViewModel.Settings.Color == ThemeColors.deeporange;
            LightBlueActive = MainViewModel.Settings.Color == ThemeColors.lightblue;
            TealActive = MainViewModel.Settings.Color == ThemeColors.teal;
            CyanActive = MainViewModel.Settings.Color == ThemeColors.cyan;
            PinkActive = MainViewModel.Settings.Color == ThemeColors.pink;
            GreenActive = MainViewModel.Settings.Color == ThemeColors.green;
            DeepPurpleActive = MainViewModel.Settings.Color == ThemeColors.deeppurple;
            IndigoActive = MainViewModel.Settings.Color == ThemeColors.indigo;
            LightGreenActive = MainViewModel.Settings.Color == ThemeColors.lightgreen;
            BlueActive = MainViewModel.Settings.Color == ThemeColors.blue;
            LimeActive = MainViewModel.Settings.Color == ThemeColors.lime;
            RedActive = MainViewModel.Settings.Color == ThemeColors.red;
            OrangeActive = MainViewModel.Settings.Color == ThemeColors.orange;
            PurpleActive = MainViewModel.Settings.Color == ThemeColors.purple;

            ThemeName = MainViewModel.Settings.DarkMode ? "Dark" : "Light";
        }



        #endregion
    }
}
