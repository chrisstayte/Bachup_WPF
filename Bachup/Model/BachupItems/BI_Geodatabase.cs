using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bachup.Model;

namespace Bachup.Model.BachupItems
{
    class BI_Geodatabase : BachupItem
    {
        public BI_Geodatabase(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        { 

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

        public override void RunBachup()
        {
            if (IsFileLocked())
                return;

            if (CheckDestinationsConnection())
            {

                // TODO: Implement a follow up asking if the user wants to backup to destinations that exist
                
            }

            foreach (string destination in Destinations)
            {
                if (Directory.Exists(destination))
                {
                    GenerateBachupLocation(destination);
                }
            }

        }

        #endregion
    }
}
