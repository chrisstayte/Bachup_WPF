using Bachup.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bachup.Model
{
    class BachupGroup : INotifyPropertyChanged
    {
        public BachupGroup(string name)
        {
            Name = name;
            ID = Guid.NewGuid();
            BachupItems = new ObservableCollection<BachupItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Properties
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

        private Guid _ID;
        public Guid ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<BachupItem> _bachupItems;
        public ObservableCollection<BachupItem> BachupItems
        {
            get
            {
                return _bachupItems;
            }
            set
            {
                _bachupItems = value;
                NotifyPropertyChanged();
            }
        }

        #region Methods

        public void AddBachupItem(BachupItem bachupItem)
        {
            BachupItems.Add(bachupItem);
            MainViewModel.SaveData();
        }

        public void RemoveBachupItem(BachupItem bachupItem)
        {
            BachupItems.Remove(bachupItem);
            MainViewModel.SaveData();
        } 

        private BachupType DetermineBachupType(string Source)
        {
            string extension = Path.GetExtension(Source);

            if (Enum.IsDefined(typeof(BachupType), extension.ToUpper()))
                return (BachupType)Enum.Parse(typeof(BachupType), extension.ToUpper());
            else
            {
                return BachupType.NotSupported;
            }
        }

        public bool DoesItemExist(string name)
        {
            return _bachupItems.FirstOrDefault(item => item.Name.ToLower() == name.ToLower()) != null;
        }

        #endregion


    }
}
