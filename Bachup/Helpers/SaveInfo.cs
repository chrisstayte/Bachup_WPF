using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Helpers
{
    static class SaveInfo
    {

        private static string _saveFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bachup");
            }
        }

        public static string SaveFolder
        {
            get
            {
                return _saveFolder;
            }
        }

        public static string SettingsFile
        {
            get
            {
                return Path.Combine(_saveFolder, "settings.json");
            }
        }

        public static string DataFile
        {
            get
            {
                return Path.Combine(_saveFolder, "bachup_data.json");
            }
        }

       
    }
}
