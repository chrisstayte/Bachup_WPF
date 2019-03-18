using Bachup.Model;
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
            bachupGroup = BachupGroupInput;
            Name = bachupGroup.Name;
        }

        private BachupGroup bachupGroup;

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
    }
}
