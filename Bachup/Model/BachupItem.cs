using Bachup.View;
using Bachup.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Bachup.Model;
using Bachup.Helpers;
using Newtonsoft.Json;

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
            AutoWeekdays = new Dictionary<Weekdays, bool>
            {
                { Weekdays.Sunday, false },
                { Weekdays.Monday, false },
                { Weekdays.Tuesday, false },
                { Weekdays.Wednesday, false },
                { Weekdays.Thursday, false },
                { Weekdays.Friday, false },
                { Weekdays.Saturday, false }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Guid _id;
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
                NotifyPropertyChanged();
            }
        }
        
        public Guid BachupGroupID { get; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }    
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

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }

        private bool _autoBachup;
        public bool AutoBachup
        {
            get { return _autoBachup; }
            set
            {
                if (_autoBachup != value)
                {
                    _autoBachup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private String _autoTime;
        public String AutoTime
        {
            get
            {
                return _autoTime;
            }
            set
            {
                if (_autoTime != value)
                {
                    _autoTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Dictionary<Weekdays, bool> _autoWeekdays;
        public Dictionary<Weekdays, bool> AutoWeekdays
        {
            get { return _autoWeekdays; }
            set
            {
                if (_autoWeekdays != value)
                {
                    _autoWeekdays = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _useFileNameForBachup;
        public bool UseFileNameForBachup
        {
            get
            {
                return _useFileNameForBachup;
            }
            set
            {
                if (_useFileNameForBachup != value)
                {
                    _useFileNameForBachup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<BachupHistory> _bachupHistory;
        public ObservableCollection<BachupHistory> BachupHistory
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

        private double _sizeInMB;
        public double SizeInMB
        {
            get { return _sizeInMB; }
            set
            {
                _sizeInMB = value;
                NotifyPropertyChanged();
            }
        }

        private bool _sourceBroken;
        public bool SourceBroken
        {
            get { return _sourceBroken; }
            set
            {
                if (_sourceBroken != value)
                {
                    _sourceBroken = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _showLastBachup;
        public bool ShowLastBachup
        {
            get { return _showLastBachup; }
            set
            {
                if (_showLastBachup != value)
                {
                    _showLastBachup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _runningBachup;
        [JsonIgnore]
        public bool RunningBachup
        {
            get { return _runningBachup; }
            set
            {
                if (_runningBachup != value)
                {
                    _runningBachup = value;
                    NotifyPropertyChanged();
                }
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

        internal async Task<bool> CheckDestinationsConnection(bool promptWithDialogToContinue)
        { 
            int missingDestinations = 0;

            foreach (string destination in Destinations)
                if (!System.IO.Directory.Exists(destination))
                {
                    missingDestinations++;
                }
                    

            if (missingDestinations == Destinations.Count())
            {
                var view = new AlertView
                { 
                    DataContext = new AlertViewModel("All destinations are missing. Add a destination.")
                };
                await DialogHost.Show(view, "RootDialog");
                return false;
            }

            if (missingDestinations > 0)
            {
                var view = new ConfirmChoiceView
                {
                    DataContext = new ConfirmChoiceViewModel("Bachup With Missing?",
                    "There are destinations missing. You can choose to bachup with connected destinations.")
                };
                return (bool)await DialogHost.Show(view, "RootDialog");
            }

            int brokenDestinaitons = 0;

            foreach (var tempFile in from string destination in Destinations
                                     let tempFile = Path.Combine(destination, "bachup.tmp")
                                     select tempFile)
            {
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
                        brokenDestinaitons++;
                    }
                }
                catch
                {
                    brokenDestinaitons++;
                }
            }



            if (brokenDestinaitons == Destinations.Count())
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("All destinations are broken. Add a destination.")
                };
                await DialogHost.Show(view, "RootDialog");
                return false;
            }

            if (brokenDestinaitons > 0)
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
            string bachupLocation = System.IO.Path.Combine(destination, UseFileNameForBachup ? Path.GetFileNameWithoutExtension(Source) : Name);
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

                if (Directory.Exists(System.IO.Path.Combine(bachupLocation, number)))
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

        internal bool CheckRequirements()
        {
            if (!CheckSourceExistence())
            {
                MainViewModel.ShowMessage($"{Name} Bachup Failed", $"Repair The Source", Notifications.Wpf.NotificationType.Error);
                return false;
            }

            if (Destinations.Count <= 0)
            {
                MainViewModel.ShowMessage($"{Name} Bachup Failed", $"There Are No Destinations", Notifications.Wpf.NotificationType.Error);
                return false;
            }

            if (IsFileLocked())
            {
                MainViewModel.ShowMessage($"{Name} Bachup Failed", $"The Item Is Locked", Notifications.Wpf.NotificationType.Error);
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
            if (CheckRequirements())
            {
                if (await CheckDestinationsConnection(true))
                {
                    RunningBachup = true;

                    if (BachupHistory == null)
                    {
                        BachupHistory = new ObservableCollection<BachupHistory>();
                    }
                    BachupHistory bachupHistory = new BachupHistory();

                    foreach (string destination in Destinations)
                    {
                        if (!Directory.Exists(destination))
                        {
                            bachupHistory.BachupDestinationStatus.Add(destination, false);
                            continue;
                        }

                        bool success = false;

                        if (ZipBachup)
                        {
                            await (Task.Run(() => {
                                success = CopyDataWithZip(destination);
                            }));
                        }
                        else
                        {
                            await (Task.Run(() =>
                            {
                                success = CopyData(destination);
                            }));
                        }
                        bachupHistory.BachupDestinationStatus.Add(destination, success);
                    }

                    bachupHistory.GetStatus();
                    DateTime completedDateTime = DateTime.Now;

                    bachupHistory.BachupDateTime = completedDateTime;
                    LastBachup = completedDateTime;
                    BachupHistory.Insert(0, bachupHistory);

                    switch (bachupHistory.Type)
                    {
                        case BachupHistoryType.noHistory:
                            break;
                        case BachupHistoryType.fullBachup:
                            MainViewModel.ShowMessage("Bached Up", $"{Name} is Bached Up", Notifications.Wpf.NotificationType.Success);
                            ShowLastBachup = true;
                            break;
                        case BachupHistoryType.partialBachup:
                            MainViewModel.ShowMessage("Bached Up", $"{Name} is partially Bached Up", Notifications.Wpf.NotificationType.Warning);
                            ShowLastBachup = true;
                            break;
                        case BachupHistoryType.failedBachup:
                            MainViewModel.ShowMessage("Bached Up", $"{Name} Failed To Bachup", Notifications.Wpf.NotificationType.Error);
                            break;
                    }
                    
                    RunningBachup = false;
                    MainViewModel.SaveData();
                }
            }
        }

        public bool CheckSourceExistence()
        {
            SourceBroken = (!Directory.Exists(Source) ^ File.Exists(Source));
            return !SourceBroken;
        }

        // These are custom to each type. Each subtype will need to override these methods and implement a custom version
        public abstract bool IsFileLocked();
        public abstract bool CopyData(string destination);
        public abstract bool CopyDataWithZip(string destination);
        public abstract void RepairSource();
        public abstract void GetSize();

        #endregion 
    }
}
