using Bachup.Model;
using Bachup.ViewModel;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Media;

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
            ShowNotifications = false;
            AutoBachupEnabled = false;

        }

        public void ResetSettings()
        {
            DarkMode = false;
            Color = ThemeColors.red;
            KeepOnTop = false;
            LastOpened = null;
            Beta = false;
            ShowNotifications = false;
            AutoBachupEnabled = false;
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

        private bool _showNotifications;
        public bool ShowNotifications
        {
            get { return _showNotifications; }
            set
            {
                if (_showNotifications != value)
                {
                    _showNotifications = value;
                    NotifyPropertyChanged();
                }
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

        private CompressionLevel _compressionLevel;
        public int CompressionLevel
        {
            get
            {
                return (int)_compressionLevel;
            }
            set
            {
                _compressionLevel = (CompressionLevel)value;
                NotifyPropertyChanged();
            }
        }

        private bool _autoBachupEnabled;
        public bool AutoBachupEnabled;

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
            var palletHelper = new PaletteHelper();
            ITheme theme = palletHelper.GetTheme();
            
            try
            {
                switch (Color)
                {
                    case ThemeColors.amber:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Amber]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Amber]);
                        break;
                    case ThemeColors.blue:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Blue]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Blue]);
                        break;
                    case ThemeColors.cyan:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Cyan]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Cyan]);
                        break;
                    case ThemeColors.deeporange:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.DeepOrange]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.DeepOrange]);
                        break;
                    case ThemeColors.deeppurple:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.DeepPurple]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.DeepPurple]);
                        break;
                    case ThemeColors.green:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Green]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Green]);
                        break;
                    case ThemeColors.indigo:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Indigo]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Indigo]);
                        break;
                    case ThemeColors.lightblue:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.LightBlue]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.LightBlue]);
                        break;
                    case ThemeColors.lightgreen:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.LightGreen]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.LightGreen]);
                        break;
                    case ThemeColors.lime:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Lime]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Lime]);
                        break;
                    case ThemeColors.orange:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Orange]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Orange]);
                        break;
                    case ThemeColors.pink:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Pink]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Pink]);
                        break;
                    case ThemeColors.purple:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Purple]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Purple]);
                        break;
                    case ThemeColors.red:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Red]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Red]);
                        break;
                    case ThemeColors.teal:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Teal]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Teal]);
                        break;
                    case ThemeColors.yellow:
                        theme.SetPrimaryColor(SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.Yellow]);
                        theme.SetSecondaryColor(SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Yellow]);
                        break;
                }

                theme.SetBaseTheme(DarkMode ? Theme.Dark : Theme.Light);

                palletHelper.SetTheme(theme);
                
                
            }
            catch
            {
                MainViewModel.Settings.ResetSettings();
                //new PaletteHelper().ReplaceAccentColor(MainViewModel.Settings.Color.ToString());
                //new PaletteHelper().ReplacePrimaryColor(MainViewModel.Settings.Color.ToString());
            }
        }

        public void SetDarkMode()
        {
            try
            {
                var palletHelper = new PaletteHelper();
                ITheme theme = palletHelper.GetTheme();
                theme.SetBaseTheme(DarkMode ? Theme.Dark : Theme.Light);
                palletHelper.SetTheme(theme);
            }
            catch
            {
                ResetSettings();
                //new PaletteHelper().SetLightDark(DarkMode);
            }
        }
        #endregion

    }
}
