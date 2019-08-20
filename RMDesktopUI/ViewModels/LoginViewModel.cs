using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Helpers;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        /*
         * Properties with Backing Fields
         */
        private string _userName;
        private string _password;
        private string _errorMessage;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                // It's going to be fired every time we change the user value
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get => _password;
            set
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }


        /*
         * Properties
         */
        public bool IsErrorVisible
        {
            get
            {
                var output = ErrorMessage?.Length > 0;

                return output;
            }
        }

        public bool CanLogIn
        {
            get
            {
                var output = UserName?.Length > 0 && Password?.Length > 0;

                return output;
            }
        }

        private readonly IAPIHelper _apiHelper;


        /*
         * Constructor
         */
        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }


        public async Task LogIn(string userName, string password)
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex) 
            {

                ErrorMessage = ex.Message;
            }
        }

    }
}
