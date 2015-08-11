using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace StateByState
{
    public enum SecondStates
    {
        Base,
        State1,
        State2,
        State3
    }

    public enum SecondStateTransitions
    {
        Base,
        State1To2,
        State2To3,
        State3To1,
        StateXToZ,
        StateYToZ,
        StateZToY,
        StateYToX
    }


    public enum SecondStates2
    {
        Base,
        StateX,
        StateY,
        StateZ
    }
    public class SecondViewModel : NotifyBase
    {
        public event EventHandler SecondCompleted;


        public string Name { get; } = "Bob";

        public IStateManager<SecondStates, SecondStateTransitions> StateManager { get; }
        public IStateManager<SecondStates2, SecondStateTransitions> StateManager2 { get; }

        public SecondViewModel()
        {
            StateManager = new BaseStateManager<SecondStates, SecondStateTransitions>
            {
                States = new Dictionary<SecondStates, IStateDefinition<SecondStates>>
                {
                    {
                        SecondStates.State1, new BaseStateDefinition<SecondStates>
                        {
                            State = SecondStates.State1
                        }
                    },
                    {
                        SecondStates.State2, new BaseStateDefinition<SecondStates>
                        {
                            State = SecondStates.State2
                        }
                    },
                    {
                        SecondStates.State3, new BaseStateDefinition<SecondStates>
                        {
                            State = SecondStates.State3
                        }
                    }
                },
                Transitions = new Dictionary<SecondStateTransitions, ITransitionDefinition<SecondStates>>
                {
                    {
                        SecondStateTransitions.State1To2,
                        new BaseTransitionDefinition<SecondStates>
                        {
                            StartState = SecondStates.State1,
                            EndState = SecondStates.State2,
                        }
                    },
                    {
                        SecondStateTransitions.State2To3,
                        new BaseTransitionDefinition<SecondStates>
                        {
                            StartState = SecondStates.State2,
                            EndState = SecondStates.State3,
                        }
                    },
                    {
                        SecondStateTransitions.State3To1,
                        new BaseTransitionDefinition<SecondStates>
                        {
                            StartState = SecondStates.State3,
                            EndState = SecondStates.State1,
                        }
                    }
                }
            };


            StateManager2 = new BaseStateManager<SecondStates2, SecondStateTransitions>
            {
                States = new Dictionary<SecondStates2, IStateDefinition<SecondStates2>>
                {
                    {
                        SecondStates2.StateX, new BaseStateDefinition<SecondStates2>
                        {
                            State = SecondStates2.StateX
                        }
                    },
                    {
                        SecondStates2.StateY, new BaseStateDefinition<SecondStates2>
                        {
                            State = SecondStates2.StateY
                        }
                    },
                    {
                        SecondStates2.StateZ, new BaseStateDefinition<SecondStates2>
                        {
                            State = SecondStates2.StateZ
                        }
                    }
                },
                Transitions = new Dictionary<SecondStateTransitions, ITransitionDefinition<SecondStates2>>
                {
                    {
                        SecondStateTransitions.StateXToZ,
                        new BaseTransitionDefinition<SecondStates2>
                        {
                            StartState = SecondStates2.StateX,
                            EndState = SecondStates2.StateZ,
                        }
                    },
                    {
                        SecondStateTransitions.StateYToZ,
                        new BaseTransitionDefinition<SecondStates2>
                        {
                            StartState = SecondStates2.StateY,
                            EndState = SecondStates2.StateZ,
                        }
                    },
                    {
                        SecondStateTransitions.StateZToY,
                        new BaseTransitionDefinition<SecondStates2>
                        {
                            StartState = SecondStates2.StateZ,
                            EndState = SecondStates2.StateY,
                        }
                    },
                     {
                        SecondStateTransitions.StateYToX,
                        new BaseTransitionDefinition<SecondStates2>
                        {
                            StartState = SecondStates2.StateY,
                            EndState = SecondStates2.StateX,
                        }
                    }

                }
            };
        }

        public async Task InitSecond()
        {
           await  StateManager.ChangeTo(SecondStates.State1);
            await StateManager2.ChangeTo(SecondStates2.StateX);
            await TaskEx.Delay(1000);
            Debug.WriteLine("Break");
        }

        public void GoBack()
        {
            SecondCompleted?.Invoke(this, EventArgs.Empty);
        }


        public void ToFirst()
        {
            StateManager.Transition(SecondStateTransitions.State1To2);

            //SecondStateManager.ChangePageState(SecondStates.State1, false);
            //SecondState2Manager.ChangePageState(SecondStates2.StateX, false);
        }
        public void ToSecond()
        {
            StateManager.Transition(SecondStateTransitions.State2To3);
            //SecondStateManager.ChangePageState(SecondStates.State2, false);
            // SecondState2Manager.ChangePageState(SecondStates2.StateY, false);
        }
        public void ToThird()
        {
            StateManager.Transition(SecondStateTransitions.State3To1);
            //SecondStateManager.ChangePageState(SecondStates.State3, false);
            //  SecondState2Manager.ChangePageState(SecondStates2.StateZ, false);
        }


        public void XtoZ()
        {
            StateManager2.Transition(SecondStateTransitions.StateXToZ);
        }
        public void YtoZ()
        {
            StateManager2.Transition(SecondStateTransitions.StateYToZ);
        }
        public void ZtoY()
        {
            StateManager2.Transition(SecondStateTransitions.StateZToY);
        }
        public void YtoX()
        {
            StateManager2.Transition(SecondStateTransitions.StateYToX);
        }

        public void Done()
        {
            SecondCompleted?.Invoke(this, EventArgs.Empty);

        }
    }
}