using Bachup.View;
using Bachup.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Principal;
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

        private ObservableCollection<DateTime> _bachupHistory;
        public ObservableCollection<DateTime> BachupHistory
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

        private bool _zipBachup;
        public bool ZipBachup
        {
            get
            {
                return _zipBachup;
            }
            set
            {
                _zipBachup = value;
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

        internal BachupType _bachupType;
        public BachupType BachupType
        {
            get
            {
                return _bachupType;
            }

        }

        internal BachupItemSourceTypes _sourceType;
        public BachupItemSourceTypes SourceType
        {
            get { return _sourceType; }
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

        internal async Task<bool> CheckDestinationsConnection(bool promptWithDialogToContinue)
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

            bool unauthorizedAccessDestinations = false;

            foreach (string destination in Destinations)
            {
                string tempFile = Path.Combine(destination, "bachup.tmp");
                try
                {
                    
                    using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                    {
                        fs.WriteByte(0xFF);
                    }

                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                    else
                    {
                        unauthorizedAccessDestinations = true;
                    }
                }
                catch
                {
                    unauthorizedAccessDestinations = true;
                }
                

            }

            if (unauthorizedAccessDestinations)
            {
                var view = new ConfirmChoiceView
                {
                    DataContext = new ConfirmChoiceViewModel("Continue?",
                "There are destinations that have no access. Continue with ones that do?")
                };
                return (bool)await DialogHost.Show(view, "RootDialog");
            }

            return true;
        }

        public string GenerateBachupLocation(string destination)
        {
            // Create Initial Named Folder
            string bachupLocation = System.IO.Path.Combine(destination, Name);
            bachupLocation = System.IO.Path.Combine(bachupLocation, CurrentDate());


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
            try
            {
                System.IO.Directory.CreateDirectory(bachupLocation);
            }
            catch
            {
                return "";
            }


            return bachupLocation;
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

        internal async Task<bool> CheckRequirements()
        {
            if (!Directory.Exists(Source) ^ File.Exists(Source))
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("Repair The Source")
                };
                await DialogHost.Show(view, "RootDialog");
                return false;
            }


            if (Destinations.Count <= 0)
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("There Are No Destinations")
                };
                await DialogHost.Show(view, "RootDialog");
                return false;
            }

            if (IsFileLocked())
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("Bachup Item Is Locked")
                };

                await DialogHost.Show(view, "RootDialog");
                return false;
            }
            return true;
        }

        internal bool HasPermission(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            try
            {
                var accessControlList = Directory.GetAccessControl(path);

                if (accessControlList == null)
                {
                    return false;
                }

                var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

                if (accessRules == null)
                {
                    return false;
                }

                foreach (FileSystemAccessRule rule in accessRules)
                {
                    if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                        continue;

                    if (rule.AccessControlType == AccessControlType.Allow)
                        writeAllow = true;
                    else if (rule.AccessControlType == AccessControlType.Deny)
                        writeDeny = true;
                }

                return writeAllow && writeDeny;



            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async void RunBachup()
        {
            if (await CheckRequirements())
            {
                if (await CheckDestinationsConnection(true))
                {
                    CopyBachupItemView view = new CopyBachupItemView()
                    {
                        DataContext = new CopyBachupItemViewModel()
                    };

                    await DialogHost.Show(view, "RootDialog", new DialogOpenedEventHandler(async (object sender, DialogOpenedEventArgs args) =>
                    {
                        DialogSession session = args.Session;

                        await Task.Run((Action)CopyData);

                        session.Close();
                    }));

                    DateTime completedDateTime = DateTime.Now;
                    LastBachup = completedDateTime;

                    if (BachupHistory == null)
                        BachupHistory = new ObservableCollection<DateTime>();

                    BachupHistory.Insert(0, completedDateTime);

                    MainViewModel.SaveData();
                }
            }
        }


        // These are custom to each type. Each subtype will need to override these methods and implement a custom version
        public abstract bool IsFileLocked();
        public abstract void CopyData();
        public abstract void RepairSource();

        #endregion 
    }
}
