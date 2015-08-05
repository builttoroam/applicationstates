using System;
using System.Collections.Generic;
using BuiltToRoam.Lifecycle;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace StateByState
{
    public enum PageStates
    {
        Base,
        Main,
        Second
    }

    public enum PageTransitions
    {
        Base,
        MainToSecond,
        SecondToMain
    }

    public class CoreApplication : NotifyBase, IHasStateManager<PageStates,PageTransitions>
    {
        public IStateManager<PageStates, PageTransitions> StateManager { get; }

        public CoreApplication()
        {

            StateManager = new ViewModelStateManager<PageStates, PageTransitions>
            {
                States = new Dictionary<PageStates, IStateDefinition<PageStates>>
                {
                    {
                        PageStates.Main, new ViewModelStateDefinition<PageStates, MainViewModel>
                        {
                            State = PageStates.Main,
                            InitialiseViewModel = async vm => await vm.Init()
                        }
                    },
                    {
                        PageStates.Second, new ViewModelStateDefinition<PageStates, SecondViewModel>
                        {
                            State = PageStates.Second,
                            InitialiseViewModel = async vm => await vm.InitSecond()
                        }
                    }
                },
                Transitions = new Dictionary<PageTransitions, ITransitionDefinition<PageStates>>
                {
                    {
                        PageTransitions.MainToSecond,
                        new ViewModelTransitionDefinition<PageStates>
                        {
                            StartState = PageStates.Main,
#pragma warning disable 1998 // Needs to return Task
                            LeavingStateViewModel = async (state, vm, cancel) => (vm as MainViewModel).Completed -= State_Completed,
#pragma warning restore 1998
                            EndState = PageStates.Second,
#pragma warning disable 1998 // Needs to return Task
                            ArrivedStateViewModel = async (state, vm) => (vm as SecondViewModel).SecondCompleted += SecondCompleted,
#pragma warning restore 1998
                        }
                    },
                    {
                        PageTransitions.SecondToMain,
                        new ViewModelTransitionDefinition<PageStates>
                        {
                            StartState = PageStates.Second,
#pragma warning disable 1998 // Needs to return Task
                            LeavingStateViewModel = async (state, vm, cancel) => (vm as SecondViewModel).SecondCompleted -= SecondCompleted,
#pragma warning restore 1998
                            EndState = PageStates.Main,
#pragma warning disable 1998 // Needs to return Task
                            ArrivedStateViewModel= async (state, vm) => (vm as MainViewModel).Completed += State_Completed,
#pragma warning restore 1998
                        }
                    }
                }
            };
        }

        public async void Start()
        {
            await StateManager.ChangeTo(PageStates.Main, false);
            var state = (StateManager as ViewModelStateManager<PageStates, PageTransitions>).CurrentViewModel as MainViewModel;
            state.Completed += State_Completed;
        }

        private async void State_Completed(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.MainToSecond);
        }

        private async void SecondCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.SecondToMain);
        }
    }
}