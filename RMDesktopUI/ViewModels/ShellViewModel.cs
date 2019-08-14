using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object> // Could also be set up more specifically
                                                    // for certain types, or even use an interface.
    {
        // Using constructor injection to pass in a new instance of loginVm
        private LoginViewModel _loginVm;
        public ShellViewModel(LoginViewModel loginVm)
        {
            _loginVm = loginVm;
            ActivateItem(_loginVm);

        }

        // Preventing from inheritance, because of virtual member call in constructor
        public sealed override void ActivateItem(object item)
        {
            base.ActivateItem(item);
        }
    }
}
