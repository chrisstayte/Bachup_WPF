using Bachup.Helpers;
using System;
using System.Reflection;

namespace Bachup.ViewModel
{
    class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            Message = "Thanks For Using Bachup";

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            //VersionNumber = String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            VersionNumber = String.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.MajorRevision);

            ShowMySiteCommand = new RelayCommand(ShowMySite);

            WelcomeMessage = String.Format("Welcome {0}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Environment.UserName.ToString().ToLower()));
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

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get
            {
                return _welcomeMessage;
            }
            set
            {
                if (_welcomeMessage != value)
                {
                    _welcomeMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Relay Commands

        public RelayCommand ShowMySiteCommand { get; private set; }

        #region Events

        private void ShowMySite(object o)
        {
            System.Diagnostics.Process.Start("http://chrisstayte.com");
        }

        #endregion
    }
}
