using Bachup.Model;
using Bachup.ViewModel;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Bachup.Model.BachupItems
{
    class BI_Shapefile : BachupItem
    {
        public BI_Shapefile(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.SHP;
            _sourceType = BachupItemSourceTypes.File;
        }

        readonly List<string> extensions = new List<string>(new string[]
        {
            "shp",
            "shx",
            "dbf",
            "prj",
            "xml",
            "sbn",
            "sbx",
            "cpg"
        });


        public override bool CopyData(string destination)
        {
           
                string bachupLocation = GenerateBachupLocation(destination);

                if (bachupLocation == "")
                    return false;

                string sourceFolder = Path.GetDirectoryName(Source);
                string fileName = Path.GetFileNameWithoutExtension(Source);
                
                foreach (string extension in extensions)
                {
                    string sourceFile = Path.Combine(sourceFolder, String.Format("{0}.{1}", fileName, extension));
                    if (File.Exists(sourceFile))
                    {
                        string destFile = Path.Combine(bachupLocation, String.Format("{0}.{1}", fileName, extension));

                        File.Copy(sourceFile, destFile);
                    }
                }
            return true;
            
        }

        public override bool CopyDataWithZip(string destination)
        {
                string bachupLocation = GenerateBachupLocation(destination);

            if (bachupLocation == "")
                return false;

                string sourceFolder = Path.GetDirectoryName(Source);
                string fileName = Path.GetFileNameWithoutExtension(Source);

                using (ZipFile zip = new ZipFile())
                {
                    string zippedBachupLoction = Path.Combine(bachupLocation, fileName + ".zip");

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

                foreach (string extension in extensions)
                    {
                        string sourceFile = Path.Combine(sourceFolder, String.Format("{0}.{1}", fileName, extension));
                        if (File.Exists(sourceFile))
                        {
                            zip.AddItem(sourceFile, "");
                        }
                    }

                    zip.Save(zippedBachupLoction);
                }
            return true;
         
        }

        public override bool IsFileLocked()
        {
            string containingFolder = Path.GetDirectoryName(Source);

            string[] files = Directory.GetFiles(containingFolder, "*.lock");

            if (files.Length == 0)
                return false;

            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);
                string[] filenameSplit = filename.Split('.');

                if (filenameSplit[0].ToLower() != Path.GetFileNameWithoutExtension(Source).ToLower())
                    continue;

                if (filenameSplit[1].ToLower() != "shp")
                    continue;

                return true;
            }

            return false;
        }

        public override void RepairSource()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "shp file(*.shp)|*.shp";
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
                SizeInMB = (info.Length / 1024f) / 1024f;
            }
        }
    }
}
