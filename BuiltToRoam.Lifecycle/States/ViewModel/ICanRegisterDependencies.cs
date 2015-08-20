using Autofac;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface ICanRegisterDependencies
    {
        void RegisterDependencies(IContainer container);
    }
}