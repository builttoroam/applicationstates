using System;
using System.Threading.Tasks;
using Autofac;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle.States
{
    public interface IApplicationRegion: ICanRegisterDependencies
    {
        string RegionId { get; set; }

        event EventHandler CloseRegion;

        Task RequestClose();


        Task Startup(IRegionManager manager);
    }
}