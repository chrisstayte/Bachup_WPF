using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model
{
    abstract class BachupItem : INotifyPropertyChanged
    {
        public BachupItem()
        {

        }

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

        private List<DateTime> _backupHistory;
        public List<DateTime> BackupHistory
        {
            get
            {
                return _backupHistory;
            }
            set
            {
                _backupHistory = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _destinations;
        public List<string> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                NotifyPropertyChanged();
            }
        }

        private List<DateTime> _scheduledBackups;
        public List<DateTime> ScheduledBackups
        {
            get { return _scheduledBackups; }
            set
            {
                _scheduledBackups = value;
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

        



        #endregion 
    }
}
