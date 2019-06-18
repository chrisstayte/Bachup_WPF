
using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;

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
            CellEditedCommand = new RelayCommand(CellEdited);
            CellValueChangedCommand = new RelayCommand(CellValueChanged);
            AddDestinationCommand = new RelayCommand(AddDestination);
            DeleteDestinationCommand = new RelayCommand(DeleteDestination);
            ShowDestinationCommand = new RelayCommand(ShowDestination);

            _bachupGroup = BachupGroupInput;

            ShowMessage = false;

            UpdateView();
        }

        // This Is For DataGrid Editing
        private string _oldValue;

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

        private string _selectedDestination;
        public string SelectedDestination
        {
            get { return _selectedDestination; }
            set
            {
                if (_selectedDestination != value)
                {
                    _selectedDestination = value;
                    NotifyPropertyChanged();
                }
                EnableDestinationButtons = value != null;
            }
        }

        private bool _enableDestinationButtons;
        public bool EnableDestinationButtons
        {
            get { return _enableDestinationButtons; }
            set
            {
                if (_enableDestinationButtons != value)
                {
                    _enableDestinationButtons = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Relay Commands
        public RelayCommand EditBachupGroupCommand { get; private set; }
        public RelayCommand DeleteBachupGroupCommand { get; private set; }
        public RelayCommand AddBachupItemCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }
        public RelayCommand CellEditedCommand { get; private set; }
        public RelayCommand CellValueChangedCommand { get; private set; }
        public RelayCommand AddDestinationCommand { get; private set; }
        public RelayCommand DeleteDestinationCommand { get; private set; }
        public RelayCommand ShowDestinationCommand { get; private set; }

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
                MainViewModel.SaveData();
                UpdateView();
            }
        }

        private async void AddBachupItem(object parameter)
        {
            var view = new AddBachupItemView
            {
                DataContext = new AddBachupItemViewModel(_bachupGroup)
            };

            await DialogHost.Show(view, "RootDialog");
            UpdateView();
        }

        private void CloseMessage(object o)
        {
            ShowMessage = false;
        }

        private async void CellEdited(object o)
        {
            DataGridCellEditEndingEventArgs args = o as DataGridCellEditEndingEventArgs;
            BachupItem bachupItem = args.EditingElement.DataContext as BachupItem;
            string newValue = bachupItem.Name;
            bachupItem.Name = _oldValue;

            if (_oldValue == newValue)
                return;

            BachupGroup bg = MainViewModel.Bachup.FirstOrDefault(item => item.ID == bachupItem.BachupGroupID);

            if (bg.DoesItemExist(newValue))
            {
                AlertView view = new AlertView
                {
                    DataContext = new AlertViewModel("Name Exists In This Group")
                };
                _ = await DialogHost.Show(view, "RootDialog");
                (args.EditingElement as TextBox).Text = _oldValue;
                return;
            }

            if (String.IsNullOrEmpty(newValue) || string.IsNullOrWhiteSpace(newValue))
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("It Must Have A Name")
                };
                _ = await DialogHost.Show(view, "RootDialog");
                (args.EditingElement
                 as TextBox).Text = _oldValue;
                return;
            }

            bachupItem.Name = newValue;
            (args.EditingElement as TextBox).Text = newValue;
            MainViewModel.SaveData();

        }

        private void CellValueChanged(object o)
        {
            DataGridPreparingCellForEditEventArgs args = o as DataGridPreparingCellForEditEventArgs;
            BachupItem bachupItem = args.EditingElement.DataContext as BachupItem;
            _oldValue = bachupItem.Name;

        }

        private async void AddDestination(object o)
        {            
            var view = new AddDestinationView
            {
                DataContext = new AddDestinationViewModel(_bachupGroup)
            };

            await DialogHost.Show(view, "RootDialog");
            
            // TODO: Show Notification
        }

        private async void DeleteDestination(object o)
        {
            string message = String.Format("Delete Destination?");
            string submessage = String.Format("{0}", SelectedDestination);

            var view = new ConfirmChoiceView
            {
                DataContext = new ConfirmChoiceViewModel(message, submessage)
            };

            var choice = await DialogHost.Show(view, "RootDialog");

            if ((bool)choice)
            {
                _bachupGroup.DeleteDestination(_selectedDestination);
                MainViewModel.SaveData();
            }
        }

        private void ShowDestination(object o)
        {
            if (Directory.Exists(_selectedDestination))
            {
                Process.Start("explorer.exe", _selectedDestination);
            }
        }

        #endregion

        #region Methods

        private void UpdateView()
        {
            ShowBachupItems = _bachupGroup.BachupItems.Count > 0;
            ShowAddBachupItems = _bachupGroup.BachupItems.Count <= 0;
        }

        #endregion

    }
}
