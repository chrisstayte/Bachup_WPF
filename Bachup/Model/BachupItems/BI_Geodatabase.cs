using System;
using System.Collections.Generic;
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

            return true;
        }
        #endregion
    }
}
