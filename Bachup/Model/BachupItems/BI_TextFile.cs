using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bachup.Model.BachupItems
{
    class BI_TextFile : BachupItem
    {
        public BI_TextFile(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.TXT;
            _sourceType = BachupItemSourceTypes.File;
        }

        #region Methods

        public override bool IsFileLocked()
        {
            return false;
        }

        public override void CopyData()
        {
            foreach (string destiation in Destinations)
            {
                if (Directory.Exists(destiation))
                {
                    string bachupLocation = GenerateBachupLocation(destiation);

                    if (bachupLocation == "")
                        continue;

                    string fileName = Path.GetFileName(Source);
                    string destFile = Path.Combine(bachupLocation, fileName);

                    File.Copy(Source, destFile);
                }
            }
        }

        public override void RepairSource()
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

        #endregion
    }
}
