using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model.BachupItems
{
    class BI_TextFile : BachupItem
    {
        public BI_TextFile(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {
            _bachupType = BachupType.TXT;
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

                    Directory.CreateDirectory(bachupLocation);

                    string fileName = Path.GetFileName(Source);
                    string destFile = Path.Combine(bachupLocation, fileName);

                    File.Copy(Source, destFile);
                }
            }
        }
        #endregion
    }
}
