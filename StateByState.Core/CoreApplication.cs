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
        SecondToMain,
        MainToThird,
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

            //            {
            //                States = new Dictionary<PageStates, IStateDefinition<PageStates>>
            //                {
            //                    {
            //                        PageStates.Main, new ViewModelStateDefinition<PageStates, MainViewModel>
            //                        {
            //                            State = PageStates.Main,
            //                            InitialiseViewModel = async vm => await vm.Init(),
            //#pragma warning disable 1998 // Needs to return Task
            //                            ChangingFromViewModel = async
            //#pragma warning restore 1998
            //                            vm =>{
            //                                vm.Completed -= State_Completed;
            //                                vm.UnableToComplete -= State_UnableToCompleted;
            //                            },
            //#pragma warning disable 1998 // Needs to return Task
            //                            ChangedToViewModel = async
            //#pragma warning restore 1998
            //                            vm =>{
            //                                vm.Completed += State_Completed;
            //                                vm.UnableToComplete += State_UnableToCompleted;
            //                            },
            //                        }
            //                    },
            //                    {
            //                        PageStates.Second, new ViewModelStateDefinition<PageStates, SecondViewModel>
            //                        {
            //                            State = PageStates.Second,
            //                            InitialiseViewModel = async vm => await vm.InitSecond(),
            //#pragma warning disable 1998 // Needs to return Task
            //                            ChangedToViewModel = async vm => vm.SecondCompleted += SecondCompleted,
            //#pragma warning restore 1998
            //#pragma warning disable 1998 // Needs to return Task
            //                            ChangingFromViewModel = async vm => vm.SecondCompleted -= SecondCompleted,
            //#pragma warning restore 1998
            //                        }
            //                    },
            //                    {
            //                        PageStates.Third, new ViewModelStateDefinition<PageStates, ThirdViewModel>
            //                        {
            //                            State = PageStates.Third,
            //                            ChangedToViewModel = async vm => {
            //                                        vm.ThirdCompleted += ThirdCompleted;
            //                                    },
            //#pragma warning disable 1998 // Needs to return Task
            //                            ChangingFromViewModel = async vm => vm.ThirdCompleted -= ThirdCompleted,
            //#pragma warning restore 1998

            //                        }
            //                    }
            //                },
            sm.Transitions = new Dictionary<PageTransitions, ITransitionDefinition<PageStates>>
                            {
                                {
                                    PageTransitions.MainToSecond,
                                    new ViewModelTransitionDefinition<PageStates>
                                    {
                                        StartState = PageStates.Main,
                                        EndState = PageStates.Second,
                                    }
                                },
                                {
                                    PageTransitions.SecondToMain,
                                    new ViewModelTransitionDefinition<PageStates>
                                    {
                                        StartState = PageStates.Second,
                                        EndState = PageStates.Main,
                                    }
                                },
                                {
                                    PageTransitions.MainToThird,
                                    new ViewModelTransitionDefinition<PageStates>
                                    {
                                        StartState = PageStates.Main,
                                        EndState = PageStates.Third,
                                    }
                                },
                                {
                                    PageTransitions.ThirdToMain,
                                    new ViewModelTransitionDefinition<PageStates>
                                    {
                                        StartState = PageStates.Third,
                                        EndState = PageStates.Main,
                                    }
                                }
                            };
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
            await StateManager.Transition(PageTransitions.MainToThird);
        }

        private async void SecondCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.SecondToMain);
        }
        private async void ThirdCompleted(object sender, EventArgs e)
        {
            await StateManager.Transition(PageTransitions.ThirdToMain);
        }
    }
}