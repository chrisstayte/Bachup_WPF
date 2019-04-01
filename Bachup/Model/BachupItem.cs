using Bachup.View;
using Bachup.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model
{
    abstract class BachupItem : INotifyPropertyChanged
    {

        public BachupItem(string name, string source, Guid groupID)
        {
            ID = Guid.NewGuid();
            DateCreated = DateTime.Now;
            BachupGroupID = groupID;
            Name = name;
            Source = source;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guid ID { get; }
        public DateTime DateCreated { get; }

        public Guid BachupGroupID { get; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _source;
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                NotifyPropertyChanged();
            }
        }

        private List<DateTime> _bachupHistory;
        public List<DateTime> BachupHistory
        {
            get
            {
                return _bachupHistory;
            }
            set
            {
                _bachupHistory = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _lastbachup;
        public DateTime LastBachup
        {
            get { return _lastbachup; }
            set
            {
                _lastbachup = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> _destinations = new ObservableCollection<string>();
        public ObservableCollection<string> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                NotifyPropertyChanged();
            }
        }

        private List<DateTime> _scheduledbachups;
        public List<DateTime> Scheduledbachups
        {
            get { return _scheduledbachups; }
            set
            {
                _scheduledbachups = value;
                NotifyPropertyChanged();
            }
        }



        #region Methods

        /// <summary>
        /// Add's Destination Path To Destination List
        /// </summary>
        /// <param name="path"></param>
        public void AddDestination(string path)
        { 
            _destinations.Add(path);
        }

        public void DeleteDestination(string path)
        {
            _destinations.Remove(path);
        }

        public bool IsDestinationADuplicate(string path)
        {
            return _destinations.Contains(path);
        }

        public async Task<bool> CheckDestinationsConnection(bool promptWithDialogToContinue)
        {
            bool missingDestination = false;

            foreach (string destination in Destinations)
                if (!System.IO.Directory.Exists(destination))
                    missingDestination = true;

            if (missingDestination)
            {
                var view = new ConfirmChoiceView
                {
                    DataContext = new ConfirmChoiceViewModel("Bachup With Missing?",
                    "There are destinations missing. You can choose to bachup with connected destinations.")
                };
                return (bool)await DialogHost.Show(view, "RootDialog");
            }

            return true;
        }

        public void GenerateBachupLocation(string destination)
        {
            // Create Initial Named Folder
            string bachupLocation = System.IO.Path.Combine(destination, Name);
            bachupLocation = System.IO.Path.Combine(bachupLocation, CurrentDate());
            System.IO.Directory.CreateDirectory(bachupLocation);

            int count = 1;
            bool exists = true;

            while (exists)
            {
                string number = "";
                if (count < 10)
                {
                    number = String.Format("0{0}", count);
                }
                else
                {
                    number = String.Format("{0}", count);
                }

                if (System.IO.Directory.Exists(System.IO.Path.Combine(bachupLocation, number)))
                    count++;
                else
                {
                    bachupLocation = System.IO.Path.Combine(bachupLocation, number);
                    exists = false;
                }
            }

            System.IO.Directory.CreateDirectory(bachupLocation);
        }

        /// <summary> Gets the current date in: 01201995 which is 01/20/1995
        /// </summary>
        /// <returns>Date as 01201995</returns>
        internal static string CurrentDate()
        {
            string dayBuffer = "";
            string monthBuffer = "";
            DateTime datetime = DateTime.Now;
            string year = datetime.Year.ToString();
            string month = datetime.Month.ToString();
            string day = datetime.Day.ToString();
            if (Convert.ToInt32(day) < 10)
                dayBuffer = "0";
            if (Convert.ToInt32(month) < 10)
                monthBuffer = "0";
            return monthBuffer + month + dayBuffer + day + year;
        }


        // These are custom to each type. Each subtype will need to override these methods and implement a custom version
        public abstract bool IsFileLocked();
        public abstract void RunBachup();

        #endregion 
    }
}
