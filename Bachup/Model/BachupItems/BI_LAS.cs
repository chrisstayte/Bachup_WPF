using Ionic.Zip;
using System;
using System.IO;
using System.Windows.Forms;

namespace Bachup.Model.BachupItems
{
    class BI_LAS : BachupItem
    {

        public BI_LAS(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.LAS;
            _sourceType = BachupItemSourceTypes.File;
        }

        #region Methods

        public override bool CopyData(string destination)
        {

                if (Directory.Exists(destination))
                {
                    string bachupLocation = GenerateBachupLocation(destination);

                if (bachupLocation == "")
                    return false;

                    string fileName = Path.GetFileName(Source);
                    string destFile = Path.Combine(bachupLocation, fileName);

                    File.Copy(Source, destFile);
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

                        string zippedBachupLocation = Path.Combine(bachupLocation, Path.GetFileNameWithoutExtension(Source) + ".zip");

                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.AddItem(Source, "");
                        zip.Save(zippedBachupLocation);
                    }
                }
            return true;
            
        }

        public override bool IsFileLocked()
        {
            return false;
        }

        public override void RepairSource()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "las file(*.las)|*.las";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    Source = openFileDialog.FileName;
                }
            }
        }
        
        public override void GetSize()
        {
            if (CheckSourceExistence())
            {
                FileInfo info = new FileInfo(Source);
                SizeInMB = (info.Length/ 1024f) / 1024f;
            }
        }

        #endregion
    }
}
