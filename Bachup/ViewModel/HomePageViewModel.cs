using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bachup.ViewModel
{
    class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            Message = "Thanks For Using Bachup";

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber = String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged();
            }
        }

        private string _versionNumber;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set
            {
                _versionNumber = value;
                NotifyPropertyChanged();
            }
        }
    }
}
