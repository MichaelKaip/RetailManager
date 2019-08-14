using System;
using Caliburn.Micro;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        /*
         * Backing fields
         */
        private string _userName;
        private string _password;

        /*
         * Public properties
         */
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                // It's going to be fired every time we change the user value
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get => _password;
            set
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public bool CanLogIn(string userName, string password)
        {
            // Includes checking for null (?)
            var output = UserName?.Length > 0 && Password?.Length > 0;

            return output;
        }

        public void LogIn(string userName, string password)
        {
            
        }

    }
}
