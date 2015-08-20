using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class BaseViewModel:NotifyBase, ICanRegisterDependencies
    {
        public UIContext UIContext { get; } = new UIContext();

        public virtual void RegisterDependencies(IContainer container)
        {
            
        }
    }
}
