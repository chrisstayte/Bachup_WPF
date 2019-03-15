using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bachup.Helpers;
using Ookii.Dialogs;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using MaterialDesignThemes.Wpf;
using Bachup.Model;

namespace Bachup.ViewModel
{
    class AddBachupGroupViewModel : BaseViewModel
    {
        public AddBachupGroupViewModel()
        {
            CancelCommand = new RelayCommand(Cancel);
            AddCommand = new RelayCommand(Add);

            ShowMessage = false;
            Message = "";
        }

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

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }

        #region Events

        //private void BrowseSourceFolder(object parameter)
        //{ 
        //    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        //    if (Directory.Exists(//SourcePath))
        //        dialog.DefaultDirectory = //SourcePath;

        //    dialog.IsFolderPicker = true;
        //    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        //    {
        //       // SourcePath = dialog.FileName;
        //    }
        //}

        //private void BrowseSourceFile(object parameter)
        //{
        //    CommonOpenFileDialog dialog = new CommonOpenFileDialog
        //    {
        //        IsFolderPicker = false
        //    };

        //    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        //    {
        //        //SourcePath = dialog.FileName;
        //    }
        //}

        private void Cancel(object parameter)
        {
            Debug.WriteLine("CANCEL BUTTON CLICKED");
            DialogHost.CloseDialogCommand.Execute(null, null);
            
        }

        private void Add(object parameter)
        {
            if (CheckRequirements())
            {
                BachupGroup bg = new BachupGroup(Name);
                DialogHost.CloseDialogCommand.Execute(bg, null);
            }
        }

        #endregion

        #region Methods

        private bool CheckRequirements()
        {
            if (_name == null)
            {
                Message = "Enter A Name";
                ShowMessage = true;
                return false;
            }

            ShowMessage = false;
            return true;
        }

        #endregion

    }
}
