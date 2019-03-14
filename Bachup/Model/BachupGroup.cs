using Bachup.Model.BachupItems;
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
    class BachupGroup : INotifyPropertyChanged
    {
        public BachupGroup(string name)
        {
            Name = name;
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

        /// <summary>
        /// Adds Existing Bachup Item
        /// </summary>
        /// <param name="bachupItem"></param>
        public void AddBachupItem(BachupItem bachupItem)
        {
            _bachupItems.Add(bachupItem);
        }

        /// <summary>
        /// Removes Bachup Item By Bachup Item
        /// </summary>
        /// <param name="bachupItem"></param>
        public void RemoveBachupItem(BachupItem bachupItem)
        {

        } 


        /// <summary>
        /// Removes Bachup Item By ID
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveBachupItem(Guid ID)
        {

        }

        // TODO: FLESH OUT BACHUP ITEM
        public void AddNewBachupItem(string name)
        {
            BachupItems.Add(new Geodatabase(name, "TEST"));
        }
        

        #endregion


    }
}
