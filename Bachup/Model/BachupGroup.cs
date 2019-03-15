using Bachup.Model.BachupItems;
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

        private async void AddBachupGroup(string message)
        {
            var view = new AddBachupGroupView
            {
                DataContext = new AddBachupGroupViewModel()
            };

            await DialogHost.Show(view, "RootDialog", ShowMessageCloseEventHandler);
        }

        private void ShowMessageCloseEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }


        #endregion


    }
}
