using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class BachupGroupViewModel : BaseViewModel
    {
        public BachupGroupViewModel(BachupGroup BachupGroupInput)
        {
            EditBachupGroupCommand = new RelayCommand(EditBachupGroup);
            DeleteBachupGroupCommand = new RelayCommand(DeleteBachupGroup);
            AddBachupItemCommand = new RelayCommand(AddBachupItem);
            CloseMessageCommand = new RelayCommand(CloseMessage);

            _bachupGroup = BachupGroupInput;

            ShowMessage = false;

            ShowBachupItems = _bachupGroup.BachupItems.Count > 0;
            ShowAddBachupItems = _bachupGroup.BachupItems.Count <= 0;
        }

        private BachupGroup _bachupGroup;
        public BachupGroup BachupGroup
        {
            get { return _bachupGroup; }
        }


        private bool _showBachupItems;
        public bool ShowBachupItems
        {
            get { return _showBachupItems; }
            set
            {
                _showBachupItems = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showAddBachupItems;
        public bool ShowAddBachupItems
        {
            get { return _showAddBachupItems; }
            set
            {
                _showAddBachupItems = value;
                NotifyPropertyChanged();
            }
        }

        private string _message;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get { return _showMessage; }
            set
            {
                _showMessage = value;
                NotifyPropertyChanged();
            }
        }

        // Relay Commands
        public RelayCommand EditBachupGroupCommand { get; private set; }
        public RelayCommand DeleteBachupGroupCommand { get; private set; }
        public RelayCommand AddBachupItemCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }

        #region Events

        private async void EditBachupGroup(object parameter)
        {
            var view = new EditBachupGroupView
            {
                DataContext = new EditBachupGroupViewModel(_bachupGroup)
            };

            await DialogHost.Show(view, "RootDialog");
        }

        private async void DeleteBachupGroup(object parameter)
        {
            string message = String.Format("Delete {0}?", BachupGroup.Name);
            string submessage = String.Format("This is not reversable. You will lose all bachup items and their history. Source files will remain in place.");

            var view = new ConfirmChoiceView
            {
                DataContext = new ConfirmChoiceViewModel(message, submessage)
            };

            var choice = await DialogHost.Show(view, "RootDialog");

            if ((bool)choice == true)
            {
                MainViewModel.Bachup.Remove(BachupGroup);
            }
        }

        private async void AddBachupItem(object parameter)
        {
            var view = new AddBachupItemView
            {
                DataContext = new AddBachupItemViewModel(_bachupGroup)
            };

            await DialogHost.Show(view, "RootDialog");
        }

        private void CloseMessage(object o)
        {
            ShowMessage = false;
        }

        #endregion

    }
}
