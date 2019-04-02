using Bachup.Helpers;
using Bachup.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class CopyBachupItemViewModel
    {

        public CopyBachupItemViewModel(BachupItem bachupItem)
        {
            _bachupItem = bachupItem;
            ContentControlLoadedCommand = new RelayCommand(ContentControlLoaded);
        }

        BachupItem _bachupItem;

        public CopyBachupItemViewModel() { }


        public RelayCommand ContentControlLoadedCommand { get; private set; }

        private void ContentControlLoaded(object o)
        {
            _bachupItem.CopyData();

            DialogHost.CloseDialogCommand.Execute(null, null);
        }

    }
}
