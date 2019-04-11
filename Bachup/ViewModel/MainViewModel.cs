using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Bachup.ViewModel
{


    /// <summary>
    /// Defines the <see cref="MainViewModel" />
    /// </summary>
    internal class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            AddBachupGroupCommand = new RelayCommand(AddBachupGroup);
            SetThemeCommand = new RelayCommand(SetTheme);
            SelectItemCommand = new RelayCommand(SelectItem);
            SetThemeColorCommand = new RelayCommand(SetThemeColor);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            Settings = new Settings();

            LoadSettings();
            LoadData();

            SetColorThemeStatus();

            SetThemeColor();
            SetThemeMode();

            
            
            SetView(null);
            
        }

        // Properties
        static public ObservableCollection<BachupGroup> Bachup { get; set; } = new ObservableCollection<BachupGroup>();
        public Settings Settings { get; set; } 

        private bool _darkMode;
        public bool DarkMode
        {
            get { return _darkMode; }
            set
            {
                _darkMode = value;
                NotifyPropertyChanged();
            }
        }

        private string _themeName;
        public string ThemeName
        {
            get { return _themeName; }
            set
            {
                _themeName = value;
                NotifyPropertyChanged();
            }
        }

        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private bool _settingsShown;
        public bool SettingsShown
        {
            get
            {
                return _settingsShown;
            }
            set
            {
                _settingsShown = value;
                NotifyPropertyChanged();
            }
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




        #endregion

        private ThemeColors _themeColor;

        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SetThemeCommand { get; private set; }
        public RelayCommand SelectItemCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }

        // Color Commands
        public RelayCommand SetThemeColorCommand { get; private set; }

        #region Events

        private async void AddBachupGroup(object o)
        {
            var view = new AddBachupGroupView {
                DataContext = new AddBachupGroupViewModel()
            };

            var test = await DialogHost.Show(view, "RootDialog");
            
            
            if (test != null)
            {
                Bachup.Add((BachupGroup)test);
                SaveData();
            }
            
        }

        private void SelectItem(object sender)
        {
            SetView(sender);
        }

        private void SetTheme(object o)
        {
            SetThemeMode();
            SaveSettings();
        }

        private void SetThemeColor(object o)
        {
            Settings.Color = (ThemeColors)o;
            _themeColor = Settings.Color;

            SetColorThemeStatus();
            SetThemeColor();
            SaveSettings();
        }

        #endregion

        #region Methods

        private void SetView(object Item)
        {
            if (Item is BachupGroup)
            {

                SelectedViewModel = new BachupGroupView()
                {
                    DataContext = new BachupGroupViewModel((BachupGroup)Item)
                };
                return;
            }


            if (Item is BachupItem)
            {
                SelectedViewModel = new BachupItemView()
                {
                    DataContext = new BachupItemViewModel((BachupItem)Item)
                };
                return;
            }

            SelectedViewModel = new HomePageView();
        }

        private void SetThemeMode()
        {
            new PaletteHelper().SetLightDark(DarkMode);
            ThemeName = DarkMode ? "Dark" : "Light";
            Settings.DarkMode = DarkMode;
        }

        private void SetThemeColor()
        {
            new PaletteHelper().ReplaceAccentColor(_themeColor.ToString());
            new PaletteHelper().ReplacePrimaryColor(_themeColor.ToString());
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(SaveInfo.DataFile))
                {
                    string save = File.ReadAllText(SaveInfo.DataFile);
                    Bachup = JsonConvert.DeserializeObject<ObservableCollection<BachupGroup>>(save, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
            }
            catch
            {

            } 
        }

        public static void SaveData()
        {
            try
            {
                if (!Directory.Exists(SaveInfo.SaveFolder))
                {
                    Directory.CreateDirectory(SaveInfo.SaveFolder);
                }

                string save = JsonConvert.SerializeObject(Bachup, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                File.WriteAllText(SaveInfo.DataFile, save);
            }
            catch
            {

            }
            
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SaveInfo.SettingsFile))
                {
                    string save = File.ReadAllText(SaveInfo.SettingsFile);
                    Settings = JsonConvert.DeserializeObject<Model.Settings>(save);
                }
            }
            catch
            {

            }
            
            DarkMode = Settings.DarkMode;
            _themeColor = Settings.Color;
        }

        private void SaveSettings()
        {
            try
            {
                if (!Directory.Exists(SaveInfo.SaveFolder))
                {
                    Directory.CreateDirectory(SaveInfo.SaveFolder);
                }

                string save = JsonConvert.SerializeObject(Settings);
                File.WriteAllText(SaveInfo.SettingsFile, save);
            }
            catch
            {

            }
        }
        
        private void SetColorThemeStatus()
        {
            YellowActive = _themeColor == ThemeColors.yellow;
            AmberActive = _themeColor == ThemeColors.amber;
            DeepOrangeActive = _themeColor == ThemeColors.deeporange;
            LightBlueActive = _themeColor == ThemeColors.lightblue;
            TealActive = _themeColor == ThemeColors.teal;
            CyanActive = _themeColor == ThemeColors.cyan;
            PinkActive = _themeColor == ThemeColors.pink;
            GreenActive = _themeColor == ThemeColors.green;
            DeepPurpleActive = _themeColor == ThemeColors.deeppurple;
            IndigoActive = _themeColor == ThemeColors.indigo;
            LightGreenActive = _themeColor == ThemeColors.lightgreen;
            BlueActive = _themeColor == ThemeColors.blue;
            LimeActive = _themeColor == ThemeColors.lime;
            RedActive = _themeColor == ThemeColors.red;
            OrangeActive = _themeColor == ThemeColors.orange;
            PurpleActive = _themeColor == ThemeColors.purple;

        }

        private void ShowSettings(object o)
        {
            SettingsShown = !_settingsShown;
        }


        #endregion


    }
}
