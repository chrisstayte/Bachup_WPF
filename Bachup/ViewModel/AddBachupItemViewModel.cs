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
    class AddBachupItemViewModel : BaseViewModel
    {

        public AddBachupItemViewModel(BachupGroup bachupGroup)
        {
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
            AddSourceFileCommand = new RelayCommand(AddSourceFile);
            AddSourceFolderCommand = new RelayCommand(AddSourceFolder);


            _bachupGroup = bachupGroup;
        }

        private BachupGroup _bachupGroup;

        // Properties (Bindings)

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


        // Relay Commands
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand AddSourceFileCommand { get; private set; }
        public RelayCommand AddSourceFolderCommand { get; private set; }

        #region Events

        private void Add(object o)
        {

        }

        private void Cancel(object o)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void AddSourceFile(object o)
        {

        }

        private void AddSourceFolder(object o)
        {

        }

        #endregion

        #region Methods




        #endregion
    }
}
