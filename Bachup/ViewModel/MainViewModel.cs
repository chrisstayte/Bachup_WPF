using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Bachup.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            AddBachupGroupCommand = new RelayCommand(AddBachupGroup);
            SetThemeCommand = new RelayCommand(SetTheme);
            SelectItemCommand = new RelayCommand(SelectItem);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            SetDarkModeCommand = new RelayCommand(DarkMode);
            ShowMySiteCommand = new RelayCommand(ShowMySite);

            Settings = new Settings();

            LoadSettings();
            LoadData();

            if (Bachup == null)
                Bachup = new ObservableCollection<BachupGroup>();

            SetColorThemeStatus();

            SetTheme();
            DarkMode();

            if (Settings.OpenToLastSelected)
            {
                Settings.DeselectAll();
                Settings.ExpandAll(false);
                SetView(ExpandAndSelectLastOpened());
            }
            else
            {
                Settings.DeselectAll();
                Settings.ExpandAll(false);
                SetView(null);
            }

            //SysTrayApp();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber = $"Version {version.Major}.{version.Minor}";
        }

        public void SysTrayApp()
        {
            ContextMenu trayMenu = new ContextMenu();

            trayMenu.MenuItems.Add("Exit", (object sender, EventArgs e) =>
            {
                Application.Exit();
                Environment.Exit(1);
            });

            TrayMenu = trayMenu;

            

        }

        private ContextMenu _trayMenu;
        public ContextMenu TrayMenu
        {
            get { return _trayMenu; }
            set
            {
                _trayMenu = value;
                NotifyPropertyChanged();
            }
        }

        // Properties
        static public ObservableCollection<BachupGroup> Bachup { get; set; } = new ObservableCollection<BachupGroup>();
        static public Settings Settings { get; set; }

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

        private string _versionNumber;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set
            {
                _versionNumber = value;
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

        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SetThemeCommand { get; private set; }
        public RelayCommand SelectItemCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand SaveSettingsCommand { get; private set; }
        public RelayCommand SetDarkModeCommand { get; private set; }
        public RelayCommand ShowMySiteCommand { get; private set; }

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
            SaveSettings();
            SaveData();
        }

        private void DarkMode(object o)
        {
            DarkMode();
            SaveSettings();
        }

        private void SetTheme(object o)
        {
           
            Settings.Color = (ThemeColors)o;

            SetTheme();

            SetColorThemeStatus();

            SaveSettings();
        }

        private void SaveSettings(object o)
        {
            SaveSettings();
        }

        private void ShowMySite(object o)
        {
            System.Diagnostics.Process.Start("http://chrisstayte.com");
        }

        #endregion

        #region Methods

        private void DarkMode()
        {
            try
            {
                new PaletteHelper().SetLightDark(Settings.DarkMode);
                ThemeName = Settings.DarkMode ? "Dark" : "Light";
            }
            catch
            {
                Settings.ResetSettings();
                new PaletteHelper().SetLightDark(Settings.DarkMode);
                ThemeName = Settings.DarkMode ? "Dark" : "Light";
            }
        }

        private void SetTheme()
        {
            try
            {
                new PaletteHelper().ReplaceAccentColor(Settings.Color.ToString());
                new PaletteHelper().ReplacePrimaryColor(Settings.Color.ToString());

            }
            catch
            {
                Settings.ResetSettings();
                new PaletteHelper().ReplaceAccentColor(Settings.Color.ToString());
                new PaletteHelper().ReplacePrimaryColor(Settings.Color.ToString());


            }
        }

        private void SetView(object item)
        {
            if (!(item is BachupGroup))
            {
                if (item is BachupItem)
                {
                    BachupItem bi = (BachupItem)item;
                    Settings.LastOpened = bi.ID;
                    SelectedViewModel = new BachupItemView()
                    {
                        DataContext = new BachupItemViewModel(bi)
                    };
                    return;
                }
                SelectedViewModel = new HomePageView();
            }
            else
            {
                BachupGroup bg = (BachupGroup)item;
                Settings.LastOpened = bg.ID;
                SelectedViewModel = new BachupGroupView()
                {
                    DataContext = new BachupGroupViewModel(bg)
                };
                return;
            }
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
        }

        public static void SaveSettings()
        {
            try
            {
                if (!Directory.Exists(SaveInfo.SaveFolder))
                {
                    Directory.CreateDirectory(SaveInfo.SaveFolder);
                }

                var save = JsonConvert.SerializeObject(Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                File.WriteAllText(SaveInfo.SettingsFile, save);
            }
            catch
            {

            }
        }
        
        private void SetColorThemeStatus()
        {
            YellowActive = Settings.Color == ThemeColors.yellow;
            AmberActive = Settings.Color == ThemeColors.amber;
            DeepOrangeActive = Settings.Color == ThemeColors.deeporange;
            LightBlueActive = Settings.Color == ThemeColors.lightblue;
            TealActive = Settings.Color == ThemeColors.teal;
            CyanActive = Settings.Color == ThemeColors.cyan;
            PinkActive = Settings.Color == ThemeColors.pink;
            GreenActive = Settings.Color == ThemeColors.green;
            DeepPurpleActive = Settings.Color == ThemeColors.deeppurple;
            IndigoActive = Settings.Color == ThemeColors.indigo;
            LightGreenActive = Settings.Color == ThemeColors.lightgreen;
            BlueActive = Settings.Color == ThemeColors.blue;
            LimeActive = Settings.Color == ThemeColors.lime;
            RedActive = Settings.Color == ThemeColors.red;
            OrangeActive = Settings.Color == ThemeColors.orange;
            PurpleActive = Settings.Color == ThemeColors.purple;

        }

        private void ShowSettings(object o) => SettingsShown = !_settingsShown;

        public static bool DoesBachupGroupExist(string name) => Bachup.FirstOrDefault(Group => Group.Name.ToLower() == name.ToLower()) != null;

        public object ExpandAndSelectLastOpened()
        {
            if (Settings.LastOpened != null)
            {
                foreach (BachupGroup bg in Bachup)
                {
                    if (bg.ID != Settings.LastOpened)
                    {
                        foreach (BachupItem bi in bg.BachupItems)
                        {
                            if (bi.ID == Settings.LastOpened)
                            {
                                bg.IsExpanded = true;
                                bi.IsSelected = true;
                                return bi;
                            }
                            continue;
                        }
                    }
                    else
                    {
                        bg.IsExpanded = false;
                        bg.IsSelected = true;
                        return bg;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
