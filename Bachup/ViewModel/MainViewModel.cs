namespace Bachup.ViewModel
{
    using Bachup.Helpers;
    using Bachup.Model;
    using Bachup.View;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

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
            bg1.AddNewBachupItem("Area 1");
            bg1.AddNewBachupItem("Area 2");
            bg1.AddNewBachupItem("Area 3");
            bg1.AddNewBachupItem("Area 4");
            bg1.AddNewBachupItem("Area 1");
            bg1.AddNewBachupItem("Area 2");
            bg1.AddNewBachupItem("Area 3");
            bg1.AddNewBachupItem("Area 1");
            bg1.AddNewBachupItem("Area 2");
            bg1.AddNewBachupItem("Area 3");
            bg1.AddNewBachupItem("Area 1");
            bg1.AddNewBachupItem("Area 2");
            bg1.AddNewBachupItem("Area 3");
            bg1.AddNewBachupItem("Area 4");
            bg1.AddNewBachupItem("Area 4");
            bg1.AddNewBachupItem("Area 4");

            BachupGroup bg2 = new BachupGroup("Project 2");
            bg2.AddNewBachupItem("Area 1");
            bg2.AddNewBachupItem("Area 2");
            bg2.AddNewBachupItem("Area 3");
            bg2.AddNewBachupItem("Area 4");
            bg2.AddNewBachupItem("Area 1");
            bg2.AddNewBachupItem("Area 2");
            bg2.AddNewBachupItem("Area 3");
            bg2.AddNewBachupItem("Area 4");
            bg2.AddNewBachupItem("Area 1");
            bg2.AddNewBachupItem("Area 2");
            bg2.AddNewBachupItem("Area 3");
            bg2.AddNewBachupItem("Area 4");
            bg2.AddNewBachupItem("Area 1");
            bg2.AddNewBachupItem("Area 2");
            bg2.AddNewBachupItem("Area 3");
            bg2.AddNewBachupItem("Area 4");

            BachupGroup bg3 = new BachupGroup("Project 3");
            bg3.AddNewBachupItem("Area 1");
            bg3.AddNewBachupItem("Area 2");
            bg3.AddNewBachupItem("Area 3");
            bg3.AddNewBachupItem("Area 4");
            bg3.AddNewBachupItem("Area 1");
            bg3.AddNewBachupItem("Area 2");
            bg3.AddNewBachupItem("Area 3");
            bg3.AddNewBachupItem("Area 4");
            bg3.AddNewBachupItem("Area 1");
            bg3.AddNewBachupItem("Area 2");
            bg3.AddNewBachupItem("Area 3");
            bg3.AddNewBachupItem("Area 4");
            bg3.AddNewBachupItem("Area 1");
            bg3.AddNewBachupItem("Area 2");
            bg3.AddNewBachupItem("Area 3");
            bg3.AddNewBachupItem("Area 4");

            Bachup = new ObservableCollection<BachupGroup>
            {
                bg1,
                bg2,
                bg3
            };

            AddBachupGroupCommand = new RelayCommand(AddBachupGroup);
            SetThemeCommand = new RelayCommand(SetTheme);

            DarkMode = true;
            ThemeName = "Dark";

            
        }

        // Properties
        public ObservableCollection<BachupGroup> Bachup { get; }


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

        



        // Relay Commands
        public RelayCommand AddBachupGroupCommand { get; private set; }
        public RelayCommand SetThemeCommand { get; private set; }
        

    



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

    }
}
