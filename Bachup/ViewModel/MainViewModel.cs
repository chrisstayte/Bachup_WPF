namespace Bachup.ViewModel
{
    using Bachup.Helpers;
    using Bachup.Model;
    using Bachup.View;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;

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
            BachupGroup bg1 = new BachupGroup("Project 1");
            bg1.AddNewBachupItem("Area 1", bg1.ID);
            bg1.AddNewBachupItem("Area 2", bg1.ID);
            bg1.AddNewBachupItem("Area 3", bg1.ID);
            bg1.AddNewBachupItem("Area 4", bg1.ID);


            BachupGroup bg2 = new BachupGroup("Project 2");
            bg2.AddNewBachupItem("Area 1", bg2.ID);
            bg2.AddNewBachupItem("Area 2", bg2.ID);
            bg2.AddNewBachupItem("Area 3", bg2.ID);
            bg2.AddNewBachupItem("Area 4", bg2.ID);


            BachupGroup bg3 = new BachupGroup("Project 3");
            bg3.AddNewBachupItem("Area 1", bg3.ID);
            bg3.AddNewBachupItem("Area 2", bg3.ID);
            bg3.AddNewBachupItem("Area 3", bg3.ID);
            bg3.AddNewBachupItem("Area 4", bg3.ID);

            Bachup.Add(bg1);
            Bachup.Add(bg2);
            Bachup.Add(bg3);

            AddBachupGroupCommand = new RelayCommand(AddBachupGroup);
            SetThemeCommand = new RelayCommand(SetTheme);
            SelectItemCommand = new RelayCommand(SelectItem);
            ChangeThemeColorCommand = new RelayCommand(ChangeThemeColor);
            

            DarkMode = true;
            ThemeName = "Dark";
            _themeColor = ThemeColors.orange;

            SetThemeColor();

            SetView(null);             
        }

        // Properties
        static public ObservableCollection<BachupGroup> Bachup { get; set; } = new ObservableCollection<BachupGroup>();


        private bool _darkMode;
        public bool DarkMode
        {
            get { return _darkMode; }
            set
            {
                _darkMode = value;
                ThemeName = value ? "Dark" : "Light";

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

        private ThemeColors _themeColor;

        



        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SetThemeCommand { get; private set; }
        public RelayCommand SelectItemCommand { get; private set; }
        public RelayCommand ChangeThemeColorCommand { get; private set; }
       
       

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
            }
            
        }


        private void SetTheme(object o)
        {
            new PaletteHelper().SetLightDark(DarkMode);
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

        private void SelectItem(object sender)
        { 
            SetView(sender);            
        }

        private void ChangeThemeColor(object sender)
        {
            ThemeColors newColor = (ThemeColors)GetNextColor();
            _themeColor = newColor;

            
            
            Debug.WriteLine(GetNextColor());
            SetThemeColor();
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
            
            if (currentColor < TotalCount)
            {
                int newColor = currentColor + 1;
                return newColor;
            }
                
            return 1;
        }


        #endregion


    }
}
