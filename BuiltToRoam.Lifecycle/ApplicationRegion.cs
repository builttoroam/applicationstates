using System;
using System.Threading.Tasks;
using Autofac;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle
{
    public class ApplicationRegion:IApplicationRegion
    {
        public event EventHandler CloseRegion;

        public string RegionId { get; set; }

#pragma warning disable 1998 // Task to allow for async override
        public virtual async Task RequestClose()
#pragma warning restore 1998
        {
        }

        protected IRegionManager Manager { get; private set; }

#pragma warning disable 1998 // Task to allow for async override
        public async Task Startup(IRegionManager manager)
#pragma warning restore 1998
        {
            Manager = manager;
            await CompleteStartup();
        }

        protected virtual async Task CompleteStartup()
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