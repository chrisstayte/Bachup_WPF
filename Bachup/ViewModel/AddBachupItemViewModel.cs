using Bachup.Helpers;
using Bachup.Model;
using Bachup.Model.BachupItems;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ShowMessage = false;
        }

        private readonly BachupGroup _bachupGroup;

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

        private string _source;
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get
            {
                return _showMessage;
            }
            set
            {
                _showMessage = value;
                NotifyPropertyChanged();
            }
        }

        private string _message;
        public string Message
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

        private bool _zipBachupItem;
        public bool ZipBachupItem
        {
            get { return _zipBachupItem; }
            set
            {
                _zipBachupItem = value;
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

            if (CheckRequirements())
            {
                BachupType bachupType = GetBachupItemType();
                if (bachupType == BachupType.NotSupported)
                {
                    Message = "Unsupported Bachup Type";
                    ShowMessage = true;
                }

                

                switch (bachupType)
                {
                    case BachupType.GDB:
                        _bachupGroup.AddBachupItem(new BI_Geodatabase(Name, Source, _bachupGroup.ID)
                        {
                            ZipBachup = _zipBachupItem
                        }
                        );       
                        break;
                    case BachupType.TXT:
                        _bachupGroup.AddBachupItem(new BI_TextFile(Name, Source, _bachupGroup.ID)
                        {
                            ZipBachup = _zipBachupItem
                        });
                        break;
                    case BachupType.NotSupported:
                        return;
                        
                }

                DialogHost.CloseDialogCommand.Execute(null, null);
            }               
        }

        private void Cancel(object o)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void AddSourceFile(object o)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    Source = openFileDialog.FileName;
                }
            }
        }

        private void AddSourceFolder(object o)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (Directory.Exists(Source))
                dialog.DefaultDirectory = Source;

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Source = dialog.FileName;
            }
        }

        #endregion

        #region Methods

        private bool CheckRequirements()
        {
            if (String.IsNullOrEmpty(Name))
            {
                Message = "Enter A Name";
                ShowMessage = true;
                return false;
            }

            if (_bachupGroup.DoesItemExist(Name))
            {
                Message = "Item With That Name Exists";
                ShowMessage = true;
                return false;
            }

            if (!Directory.Exists(Source) ^ File.Exists(Source))
            {
                Message = "Source Path Does Not Exist";
                ShowMessage = true;
                return false;
            }

            if (String.IsNullOrEmpty(Source))
            {
                Message = "Enter A Source Path";
                ShowMessage = true;
                return false;
            }

            ShowMessage = false;
            return true;
        }

        private BachupType GetBachupItemType()
        {
            string extension = Path.GetExtension(Source).Replace(".", "");

            switch (extension.ToLower())
            {
                case "gdb":
                    return BachupType.GDB;
                case "txt":
                    return BachupType.TXT;
                default:
                    return BachupType.NotSupported;
            }
        }

        #endregion
    }
}
