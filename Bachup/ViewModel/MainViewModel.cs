using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Squirrel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bachup.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        public MainViewModel() : base()
        {
            AddBachupGroupCommand = new RelayCommand(AddBachupGroup);
            SelectItemCommand = new RelayCommand(SelectItem);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            SaveSettingsCommand = new RelayCommand(SaveSettings);

            Settings = new Settings();

            LoadSettings();
            LoadData();

            Settings.SetTheme();
            Settings.SetDarkMode();

            if (Bachup == null)
                Bachup = new ObservableCollection<BachupGroup>();

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

            SysTrayApp();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber = $"Version {version.Major}.{version.Minor}";

            CheckForUpdates();
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

        private bool _rightDrawerShown;
        public bool RightDrawerShown
        {
            get
            {
                return _rightDrawerShown;
            }
            set
            {
                _rightDrawerShown = value;
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

        private object _rightDrawerContent;
        public object RightDrawerContent
        {
            get
            {
                return _rightDrawerContent;
            }
            set
            {
                if (_rightDrawerContent != value)
                {
                    _rightDrawerContent = value;
                    NotifyPropertyChanged();
                }
            }
        }


        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SelectItemCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand SaveSettingsCommand { get; private set; }


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

        private void SaveSettings(object o)
        {
            SaveSettings();
        }

        #endregion

        #region Methods

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

        private void ShowSettings(object o)
        {
            RightDrawerContent = new SettingsView()
            {
                DataContext = new SettingsViewModel()
            };
            RightDrawerShown = true;
        }  

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

        private async Task CheckForUpdates()
        {
            using (var manager = new UpdateManager(@"G:\GS\Users\Stayte\Tools\Bachup_Squirrel"))
            {
                await manager.UpdateApp();
            }
        }

        #endregion
    }
}
