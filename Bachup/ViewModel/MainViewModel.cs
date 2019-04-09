using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
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
            ChangeThemeColorCommand = new RelayCommand(ChangeThemeColor);

            Settings = new Settings();

            LoadSettings();
            LoadData();

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

        private double _windowTop;
        public double WindowTop
        {
            get { return _windowTop; }
            set
            {
                _windowTop = value;
                NotifyPropertyChanged();
            }
        }

        private ThemeColors _themeColor;

        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SetThemeCommand { get; private set; }
        public RelayCommand ChangeThemeColorCommand { get; private set; }
        public RelayCommand SelectItemCommand { get; private set; }

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

        private void ChangeThemeColor(object sender)
        {
            Settings.Color = (ThemeColors)GetNextColor();
            _themeColor = Settings.Color;

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

        private int GetNextColor()
        {
            int currentColor = (int)_themeColor;
            int TotalCount = Enum.GetNames(typeof(ThemeColors)).Length;
            int newColor;

            if (currentColor < TotalCount)
            {
                newColor = currentColor + 1;
            }
            else
            {
                newColor = 1;
            }

            return newColor;
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

        #endregion


    }
}
