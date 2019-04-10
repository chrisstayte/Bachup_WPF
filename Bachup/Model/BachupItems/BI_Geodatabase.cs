using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bachup.Model;
using Bachup.View;
using Bachup.ViewModel;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Bachup.Model.BachupItems
{
    class BI_Geodatabase : BachupItem
    {

        public BI_Geodatabase(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.GDB;
            _sourceType = BachupItemSourceTypes.Folder;
        }

        #region Methods

        public override bool IsFileLocked()
        {
            string[] files = System.IO.Directory.GetFiles(Source);

            foreach (string file in files)
            {
                string filename = System.IO.Path.GetFileName(file);
                if (Path.GetExtension(filename).ToLower().Equals(".lock"))
                    return true;
            }

            return false;
        }

        public override void CopyData()
        {
            foreach (string destination in Destinations)
            {
                if (Directory.Exists(destination))
                {
                    string bachupLocation = GenerateBachupLocation(destination);

                    string geodatabaseName = Path.GetFileName(Source);

                    string outputGeodatabase = System.IO.Path.Combine(bachupLocation, geodatabaseName);

                    Directory.CreateDirectory(outputGeodatabase);

                    string[] files = Directory.GetFiles(Source);

                    Parallel.ForEach(files, (currentFile) =>
                    {
                        string fileName = Path.GetFileName(currentFile);
                        string destFile = Path.Combine(outputGeodatabase, fileName);
                        File.Copy(currentFile, destFile, true);
                    });

                }
            }
        }

        public override void RepairSource()
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
    }
}
