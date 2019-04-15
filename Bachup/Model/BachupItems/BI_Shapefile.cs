using Ionic.Zip;
using System;
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


        public override void CopyData()
        {
            throw new NotImplementedException();
        }

        public override void CopyDataWithZip()
        {
            throw new NotImplementedException();
        }

        public override bool IsFileLocked()
        {
            throw new NotImplementedException();
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
    }
}
