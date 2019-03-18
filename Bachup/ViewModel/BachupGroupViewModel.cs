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

            _bachupGroup = BachupGroupInput;

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

        // Relay Commands
        public RelayCommand EditBachupGroupCommand { get; private set; }
        public RelayCommand DeleteBachupGroupCommand { get; private set; }

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

            var view = new ConfirmChoiceView {
                DataContext = ConfirmChoiceViewModel(message, submessage);
            }

            MainViewModel.DeleteBachupGroup(BachupGroup);
        }

        #endregion

    }
}
