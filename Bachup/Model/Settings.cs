using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.Model
{
    class Settings
    {
        public Settings()
        {
            DarkMode = false;
            Color = ThemeColors.red;
        }

        private bool _darkMode;
        public  bool DarkMode
        {
            get { return _darkMode; }
            set { _darkMode = value; }
        }

        private  ThemeColors _color;
        public  ThemeColors Color
        {
            get { return _color;  }
            set {  _color = value; }
        }

       
    }
}
