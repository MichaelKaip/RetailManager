using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        // Using constructor injection to pass in a new instance of
        // loginVm and activating it immediately after storing it.
        private IEventAggregator _events;
        private SalesViewModel _salesVM;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {
            _events = events;
            _salesVM = salesVM;

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
        }
    }
}
