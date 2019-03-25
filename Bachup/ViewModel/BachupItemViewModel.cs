using Bachup.Helpers;
using Bachup.Model;
using Bachup.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bachup.ViewModel
{
    class BachupItemViewModel : BaseViewModel
    {

        public BachupItemViewModel(BachupItem item)
        {
            // Connect Commands On Creations
            BachupBachupItemCommand = new RelayCommand(BachupBachupItem);
            AddDestinationCommand = new RelayCommand(AddDestination);
            DeleteDestinationCommand = new RelayCommand(DeleteDestination);
            ShowSourceCommand = new RelayCommand(ShowSource);

            BachupItem = item;
            EnableDeleteButton = false;
            

        }

        private BachupItem _bachupItem;
        public BachupItem BachupItem
        {
            get { return _bachupItem; }
            set
            {
                _bachupItem = value;
                NotifyPropertyChanged();
            }
        }

        public string LastBachup
        {
            get
            {
                return String.Format("Last Bachup: {0}", BachupItem.LastBachup);
            }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set
            {
                _isSaving = value;
                NotifyPropertyChanged();
            }
        }

        private double _saveProgress;
        public double SaveProgress
        {
            get { return _saveProgress; }
            set
            {
                _saveProgress = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isSaveComplete;
        public bool IsSaveComplete
        {
            get { return _isSaveComplete; }
            set
            {
                _isSaveComplete = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedDestination;
        public String SelectedDestination
        {
            get
            {
                return _selectedDestination;
            }
            set
            {
                _selectedDestination = value;
                if (value != null)
                {
                    EnableDeleteButton = true;
                }
                NotifyPropertyChanged();
            }
        }

        private bool _enableDeleteButton;
        public bool EnableDeleteButton
        {
            get
            {
                return _enableDeleteButton;
            }
            set
            {
                _enableDeleteButton = value;
                NotifyPropertyChanged();
            }
        }

        // Relay Commands

        public RelayCommand BachupBachupItemCommand { get; private set; }
        public RelayCommand AddDestinationCommand { get; private set; }
        public RelayCommand DeleteDestinationCommand { get; private set; }
        public RelayCommand ShowSourceCommand { get; private set; }

        #region Events

        private void BachupBachupItem(object parameter)
        {
            if (IsSaveComplete == true)
            {
                IsSaveComplete = false;
                return;
            }

            if (SaveProgress != 0) return;

            var started = DateTime.Now;
            IsSaving = true;

            new DispatcherTimer(
                   TimeSpan.FromMilliseconds(50),
                   DispatcherPriority.Normal,
                   new EventHandler((o, e) =>
                   {
                       var totalDuration = started.AddSeconds(3).Ticks - started.Ticks;
                       var currentProgress = DateTime.Now.Ticks - started.Ticks;
                       var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                       SaveProgress = currentProgressPercent;

                       if (SaveProgress >= 100)
                       {
                           IsSaveComplete = true;
                           IsSaving = false;
                           SaveProgress = 0;
                           ((DispatcherTimer)o).Stop();
                       }

                   }), Dispatcher.CurrentDispatcher);
        }

        private async void DeleteDestination(object parameter)
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
                _bachupItem.DeleteDestination(_selectedDestination);
                EnableDeleteButton = false;
            }
        }

        private async void AddDestination(object parameter)
        {
            var view = new AddDestinationView
            {
                DataContext = new AddDestinationViewModel(_bachupItem)
            };

            await DialogHost.Show(view, "RootDialog");
        }

        public void ShowSource(object parameter)
        {
            Process.Start("explorer.exe", _bachupItem.Source);
        }

        #endregion

        
    }
}
