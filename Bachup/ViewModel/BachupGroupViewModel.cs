
using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
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

        // Relay Commands
        public RelayCommand EditBachupGroupCommand { get; private set; }
        public RelayCommand DeleteBachupGroupCommand { get; private set; }
        public RelayCommand AddBachupItemCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }
        public RelayCommand CellEditedCommand { get; private set; }
        public RelayCommand CellValueChangedCommand { get; private set; }

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
            BachupItem bachupItem = (args.EditingElement.DataContext as BachupItem);
            string newValue = bachupItem.Name;
            bachupItem.Name = _oldValue;

            if (_oldValue == newValue)
                return;

            BachupGroup bg = MainViewModel.Bachup.FirstOrDefault(item => item.ID == bachupItem.BachupGroupID);

            if (bg.DoesItemExist(newValue))
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("Name Exists In This Group")
                };
                await DialogHost.Show(view, "RootDialog");
                (args.EditingElement as TextBox).Text = _oldValue;
                return;

            }

            if (String.IsNullOrEmpty(newValue) || string.IsNullOrWhiteSpace(newValue))
            {
                var view = new AlertView
                {
                    DataContext = new AlertViewModel("It Must Have A Name")
                };
                await DialogHost.Show(view, "RootDialog");
                (args.EditingElement as TextBox).Text = _oldValue;
                return;
            }

            bachupItem.Name = newValue;
            (args.EditingElement as TextBox).Text = newValue;
            MainViewModel.SaveData();

        }

        private void CellValueChanged(object o)
        {
            DataGridPreparingCellForEditEventArgs args = o as DataGridPreparingCellForEditEventArgs;
            BachupItem bachupItem = (args.EditingElement.DataContext as BachupItem);
            _oldValue = bachupItem.Name;

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
