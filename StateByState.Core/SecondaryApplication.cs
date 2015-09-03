using System;
using System.Threading.Tasks;
using Autofac;
using BuiltToRoam.Lifecycle;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace StateByState
{
    public enum SecondaryStates
    {
        Base,
        Main
    }
    public class SecondaryApplication : ApplicationRegion, IHasStateManager<SecondaryStates, PageTransitions>
    {
        public IStateManager<SecondaryStates, PageTransitions> StateManager { get; }

        public SecondaryApplication()
        {

            var sm = new ViewModelStateManager<SecondaryStates, PageTransitions>();

            sm.DefineViewModelState<SecondardyMainViewModel>(SecondaryStates.Main)
                .WhenChangedTo(vm =>
                {
                    vm.Done += State_Completed;
                })
                .WhenChangingFrom(vm =>
                {
                    vm.Done -= State_Completed;
                });



            StateManager = sm;

        }


        public override  void RegisterDependencies(IContainer container)
        {
            (StateManager as ICanRegisterDependencies)?.RegisterDependencies(container);
        }


        private void State_Completed(object sender, EventArgs e)
        {
            OnCloseRegion();
        }

        protected async override Task CompleteStartup()
        {
            await base.CompleteStartup();

            await StateManager.ChangeTo(SecondaryStates.Main);
        }
    }
}