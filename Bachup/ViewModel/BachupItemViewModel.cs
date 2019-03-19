using Bachup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class BachupItemViewModel : BaseViewModel
    {

        public BachupItemViewModel(BachupItem item)
        {

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

    }
}
