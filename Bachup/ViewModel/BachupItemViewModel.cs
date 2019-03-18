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
            _name = item.Name;
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

    }
}
