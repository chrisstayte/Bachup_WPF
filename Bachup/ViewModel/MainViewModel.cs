namespace Bachup.ViewModel
{
    using Bachup.Model;
    using System.Collections.ObjectModel;

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

            Bachup = new ObservableCollection<BachupGroup>
            {
                bg1,
                bg2
            };

            
        }

        /// <summary>
        /// Defines the Bachup
        /// </summary>
        public ObservableCollection<BachupGroup> Bachup { get; }
    }
}
