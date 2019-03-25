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

        public BachupItem(string name, string source)
        {
            ID = Guid.NewGuid();
            DateCreated = DateTime.Now;
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

        public bool CheckDestinationExistence(string path)
        {
            return _destinations.Contains(path);
        }

        #endregion 
    }
}
