using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        // Using constructor injection to pass in a new instance of
        // loginVm and activating it immediately after storing it.
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesVM;
        private readonly ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public bool IsLoggedIn 
        {
            get
            {
                var output = string.IsNullOrWhiteSpace(_user.Token) == false;

                return output;
            }
        }

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;

            _events.Subscribe(this);
          
            ActivateItem(IoC.Get<LoginViewModel>());

        }

        // Preventing from inheritance, because of virtual member call in constructor
        public sealed override void ActivateItem(object item)
        {
            base.ActivateItem(item);
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_salesVM);

            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();

            ActivateItem(IoC.Get<LoginViewModel>());

            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
