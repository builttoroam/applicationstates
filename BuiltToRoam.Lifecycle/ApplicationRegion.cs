using System;
using System.Threading.Tasks;
using Autofac;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle
{
    public class ApplicationRegion:IApplicationRegion, ICanRegisterDependencies
    {
        public event EventHandler CloseRegion;

        public string RegionId { get; set; }

#pragma warning disable 1998 // Task to allow for async override
        public virtual async Task RequestClose()
#pragma warning restore 1998
        {
        }


#pragma warning disable 1998 // Task to allow for async override
        public virtual async Task Startup()
#pragma warning restore 1998
        {
        }

        protected void OnCloseRegion()
        {
            CloseRegion.SafeRaise(this);
        }

        public virtual void RegisterDependencies(IContainer container)
        {
            
        }
    }
}