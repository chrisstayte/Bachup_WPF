using Bachup.ViewModel;
using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model.BachupItems
{
    class BI_DIR : BachupItem
    {
        public BI_DIR(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.DIR;
            _sourceType = BachupItemSourceTypes.Folder;
        }

        #region Methods

        public override bool IsFileLocked()
        {
            return false;
        }

        public override bool CopyData(string destination)
        {
            try
            {
                string bachupLocation = GenerateBachupLocation(destination);

                if (bachupLocation == "")
                    return false;

                string folderName = Path.GetFileName(Source);
                bachupLocation = System.IO.Path.Combine(bachupLocation, folderName);

                Directory.CreateDirectory(bachupLocation);

                var diSource = new DirectoryInfo(Source);
                var diTarget = new DirectoryInfo(bachupLocation);

                CopyAll(diSource, diTarget);
            }catch
            {
                return false;
            }

            return true;
        }

        public override bool CopyDataWithZip(string destination)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    string bachupLocation = GenerateBachupLocation(destination);

                    if (bachupLocation == "")
                        return false;

                    string zippedBachupLocation = Path.Combine(bachupLocation, Path.GetFileName(Source) + ".zip");

                    switch (MainViewModel.Settings.CompressionLevel)
                    {
                        case (int) CompressionLevel.Compression:
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                            break;
                        case (int) CompressionLevel.Speed:
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                            break;
                        case (int) CompressionLevel.Default:
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                            break;
                    }

                    zip.AddDirectory(Source);
                    zip.Save(zippedBachupLocation);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
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

        public override void GetSize()
        {
            DirectoryInfo di = new DirectoryInfo(Source);
            
        }


        #endregion
    }
}
