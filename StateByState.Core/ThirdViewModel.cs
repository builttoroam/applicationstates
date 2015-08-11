using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle;
using BuiltToRoam.Lifecycle.States;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace StateByState
{
    public enum ThirdStates
    {
        Base,
        One,
        Two,
        Three,
        Four
    }

    public enum ThirdTransitions
    {
        Base,
        OneToTwo,
        TwoToThree,
        ThreeToFour,
        FourToOne
    }

    public class ThirdViewModel:NotifyBase, IHasStateManager<ThirdStates,ThirdTransitions>
    {
        public event EventHandler ThirdCompleted;

        public IStateManager<ThirdStates, ThirdTransitions> StateManager { get; }

        public ThirdViewModel()
        {

            StateManager = new ViewModelStateManager<ThirdStates, ThirdTransitions>
            {
                States = new Dictionary<ThirdStates, IStateDefinition<ThirdStates>>
                {
                    {
                        ThirdStates.One, new ViewModelStateDefinition<ThirdStates, ThirdOneViewModel>
                        {
                            State = ThirdStates.One
                        }
                    },
                    {
                        ThirdStates.Two, new ViewModelStateDefinition<ThirdStates, ThirdTwoViewModel>
                        {
                            State = ThirdStates.Two
                        }
                    },
                    {
                        ThirdStates.Three, new ViewModelStateDefinition<ThirdStates, ThirdThreViewModel>
                        {
                            State = ThirdStates.Three
                        }
                    },
                    {
                        ThirdStates.Four, new ViewModelStateDefinition<ThirdStates, ThirdFourViewModel>
                        {
                            State = ThirdStates.Four
                        }
                    }
                },
                Transitions = new Dictionary<ThirdTransitions, ITransitionDefinition<ThirdStates>>
                {
                    {
                        ThirdTransitions.OneToTwo,
                        new ViewModelTransitionDefinition<ThirdStates>
                        {
                            StartState = ThirdStates.One,
                            EndState = ThirdStates.Two,
                        }
                    },
                     {
                        ThirdTransitions.TwoToThree,
                        new ViewModelTransitionDefinition<ThirdStates>
                        {
                            StartState = ThirdStates.Two,
                            EndState = ThirdStates.Three,
                        }
                    },
                      {
                        ThirdTransitions.ThreeToFour,
                        new ViewModelTransitionDefinition<ThirdStates>
                        {
                            StartState = ThirdStates.Three,
                            EndState = ThirdStates.Four,
                        }
                    },
                       {
                        ThirdTransitions.FourToOne,
                        new ViewModelTransitionDefinition<ThirdStates>
                        {
                            StartState = ThirdStates.Four,
                            EndState = ThirdStates.One,
                        }
                    }
                }
            };
        }

        public async Task Start()
        {
            await StateManager.ChangeTo(ThirdStates.One);
        }

        public async Task One()
        {
            await StateManager.Transition(ThirdTransitions.OneToTwo);
        }

        public async Task Two ()
        {
            await StateManager.Transition(ThirdTransitions.TwoToThree);

        }

        public async Task Three()
        {

            await StateManager.Transition(ThirdTransitions.ThreeToFour);
        }
        public async Task Four()
        {
            await StateManager.Transition(ThirdTransitions.FourToOne);

        }
    }

        public class ThirdOneViewModel : NotifyBase { public string Title => "One"; }
    public class ThirdTwoViewModel : NotifyBase { public string Title => "Two"; }
    public class ThirdThreViewModel : NotifyBase { public string Title => "Three"; }
    public class ThirdFourViewModel : NotifyBase { public string Title => "Four"; }
}
