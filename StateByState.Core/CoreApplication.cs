using System;
using System.Collections.Generic;
using System.Diagnostics;
using BuiltToRoam;
using BuiltToRoam.Lifecycle;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace StateByState
{
    public enum PageStates
    {
        Base,
        Main,
        Second,
        Third
    }

    public enum PageTransitions
    {
        Base,
        MainToSecond,
        AnyToMain,
        ThirdToMain
    }

    public class CoreApplication : NotifyBase, IHasStateManager<PageStates, PageTransitions>
    {
        public IStateManager<PageStates, PageTransitions> StateManager { get; }

        public CoreApplication()
        {

            var sm = new ViewModelStateManager<PageStates, PageTransitions>();

            #region State Definition - Main
            sm.DefineViewModelState<MainViewModel>(PageStates.Main)
                .Initialise(async vm =>
                {
                    Debug.WriteLine("VM State: Init");
                    await vm.Init();
                })
                .WhenChangedTo(vm => {
                    Debug.WriteLine("VM State: When Changed To");
                    vm.Completed += State_Completed;
                     vm.UnableToComplete += State_UnableToCompleted;
                 })
                 .WhenAboutToChange((vm,cancel) => Debug.WriteLine($"VM State: About to Change - {cancel.Cancel}"))
                 .WhenChangingFrom(vm => {
                     Debug.WriteLine("VM State: When Changing From");
                     vm.Completed -= State_Completed;
                     vm.UnableToComplete -= State_UnableToCompleted;
                 })
                .WhenAboutToChange((cancel) => Debug.WriteLine($"State: About to Change - {cancel.Cancel}"))
                .WhenChangingFrom(async () => Debug.WriteLine($"State: Changing"))
                .WhenChangedTo(() => Debug.WriteLine($"State: Changing"));
            #endregion

            #region State Definition - Second
            sm.DefineViewModelState<SecondViewModel>(PageStates.Second)
                .Initialise(async vm => await vm.InitSecond())
                .WhenChangedTo(vm => {
                                         vm.SecondCompleted += SecondCompleted;
                })
                .WhenChangingFrom(vm => {
                                            vm.SecondCompleted -= SecondCompleted;
                });
            #endregion

            #region State Definition - Third
            sm.DefineViewModelState<ThirdViewModel>(PageStates.Third)
                .WhenChangedTo(vm => {
                    vm.ThirdCompleted += ThirdCompleted;
                })
                .WhenChangingFrom(vm => {
                    vm.ThirdCompleted -= ThirdCompleted;
                });
            #endregion


            StateManager = sm;


            sm.DefineTransition(PageTransitions.MainToSecond)
                .From(PageStates.Main)
                .To(PageStates.Second);

            sm.DefineTransition(PageTransitions.AnyToMain)
                .To(PageStates.Main);

            sm.DefineTransition(PageTransitions.ThirdToMain)
                .From(PageStates.Third)
                .To(PageStates.Main);

    }

    public async void Start()
        {
            await StateManager.ChangeTo(PageStates.Main, false);
            //var state = (StateManager as ViewModelStateManager<PageStates, PageTransitions>).CurrentViewModel as MainViewModel;
            //state.Completed += State_Completed;
            //state.UnableToComplete += State_UnableToCompleted;
        }

        private async void State_Completed(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.MainToSecond);
        }
        private async void State_UnableToCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageStates.Third);
        }

        private async void SecondCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.AnyToMain);
        }
        private async void ThirdCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.ThirdToMain);
        }
    }
}