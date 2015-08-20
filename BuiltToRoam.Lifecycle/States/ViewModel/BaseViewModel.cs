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
