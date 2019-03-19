using Bachup.Helpers;
using Bachup.Model;
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

            BachupItem = item;

            

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

        // Relay Commands

        public RelayCommand BachupBachupItemCommand { get; private set; }



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


        #endregion
    }
}
