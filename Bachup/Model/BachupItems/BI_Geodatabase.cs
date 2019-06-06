using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public override bool CopyData(string destination)
        {

        if (Directory.Exists(destination))
        {
            string bachupLocation = GenerateBachupLocation(destination);

                if (bachupLocation == "")
                    return false ;

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
            return true;
            
        }

        public override bool CopyDataWithZip(string destination)
        {

                if (Directory.Exists(destination))
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        string bachupLocation = GenerateBachupLocation(destination);

                    if (bachupLocation == "")
                        return false;

                        string zippedBachupLocation = Path.Combine(bachupLocation, Path.GetFileName(Source) + ".zip");

                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.AddDirectory(Source);
                        zip.Save(zippedBachupLocation);
                    }
                }
            return true;
            
            
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
            if (CheckSourceExistence())
            {
                DirectoryInfo info = new DirectoryInfo(Source);
                SizeInMB = (info.EnumerateFiles().Sum(file => file.Length) / 1024f )  / 1024f;
            }
        }

        #endregion
    }
}
