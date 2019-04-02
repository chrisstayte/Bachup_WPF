using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model.BachupItems
{
    class BI_TextFile : BachupItem
    {
        public BI_TextFile(string name, string source, Guid bachupGroupID) : base(name, source, bachupGroupID)
        {

        }

        public override void CopyData()
        {
            throw new NotImplementedException();
        }

        #region Methods

        public override bool IsFileLocked()
        {
            throw new NotImplementedException();
        }

        public override void RunBachup()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
