using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States
{
    public class BaseStateManager<TState, TTransition> :
        NotifyBase, IStateManager<TState, TTransition>
        where TState : struct
        where TTransition : struct
    {
        public event EventHandler<StateEventArgs<TState>> StateChanged;

        public TState CurrentState { get; private set; }

        public IDictionary<TState, IStateDefinition<TState>> States { get; set; } = new Dictionary<TState, IStateDefinition<TState>>();

        public IDictionary<TTransition, ITransitionDefinition<TState>> Transitions { get; set; } = new Dictionary<TTransition, ITransitionDefinition<TState>>();

        public virtual IStateDefinition<TState> DefineState(TState state)
        {
            var stateDefinition = new BaseStateDefinition<TState> { State = state };
            return DefineState(stateDefinition);
        }

        public virtual IStateDefinition<TState> DefineState(IStateDefinition<TState> stateDefinition)
        {
            States[stateDefinition.State] = stateDefinition;
            return stateDefinition;
        }

        public virtual ITransitionDefinition<TState> DefineTransition(TTransition transition)
        {
            var transitionDefinition = new BaseTransitionDefinition<TState>();
            Transitions[transition] = transitionDefinition;
            return transitionDefinition;
        }

        public async Task<bool> ChangeTo(TState newState, bool useTransitions = true)
        {
            var current = CurrentState;
            if (current.Equals(newState)) return true;


            if (!current.Equals(default(TState)))
            {
                var currentStateDef = States[current];
                var cancel = new CancelEventArgs();
                if (currentStateDef.AboutToChangeFrom != null)
                {
                    await currentStateDef.AboutToChangeFrom(cancel);
                }
                if (cancel.Cancel) return false;
            }


            try
            {

                var proceed = await ChangeToState(current, newState);
                if (!proceed) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            CurrentState = newState;

            var newStateDef = States[newState];
            if (newStateDef.ChangedTo != null)
            {
                await newStateDef.ChangedTo();
            }

            try
            {
                StateChanged?.Invoke(this, new StateEventArgs<TState>(newState, useTransitions));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // Ignore any errors caused by the event being raised, as 
                // the state change has still occurred
            }
            return true;
        }

#pragma warning disable 1998 // Returns a Task so that overrides can do async work
        protected virtual async Task<bool> ChangeToState(TState oldState, TState newState)
#pragma warning restore 1998
        {
            if (!oldState.Equals(default(TState)))
            {
                var currentStateDef = States[oldState];
                if (currentStateDef.ChangingFrom != null)
                {
                    await currentStateDef.ChangingFrom();
                }
            }


            return true;
        }

        protected virtual ITransitionDefinition<TState> CreateDefaultTransition()
        {
            return new BaseTransitionDefinition<TState>();
        }

        public async Task<bool> Transition(TState newState, bool useTransition = true)
        {
            var transition = CreateDefaultTransition();
            transition.EndState = newState;
            return await InternalTransition(transition, useTransition);
        }

        public async Task<bool> Transition(TTransition transitionDef, bool useTransition = true)
        {
            var transition = Transitions[transitionDef];
            return await InternalTransition(transition, useTransition);
        }

        private async Task<bool> InternalTransition(ITransitionDefinition<TState> transition, bool useTransition)
        {

            if (!transition.StartState.Equals(CurrentState) && !transition.StartState.Equals(default(TState))) return false;
            var cancel = new CancelEventArgs();
            await LeavingState(transition, CurrentState, cancel);

            if (cancel.Cancel) return false;
            await ArrivingState(transition);

            if (!await ChangeTo(transition.EndState)) return false;
            await ArrivedState(transition, CurrentState);
            return true;
        }

        protected async virtual Task ArrivedState(ITransitionDefinition<TState> transition, TState currentState)
        {
            if (transition.ArrivedState != null)
            {
                await transition.ArrivedState(currentState);
            }
        }

        protected async virtual Task LeavingState(ITransitionDefinition<TState> transition, TState currentState, CancelEventArgs cancel)
        {
            if (transition.LeavingState != null)
            {
                await transition.LeavingState(currentState, cancel);
            }
        }

        protected async virtual Task ArrivingState(ITransitionDefinition<TState> transition)
        {
            if (transition.ArrivingState != null)
            {
                await transition.ArrivingState(transition.EndState);
            }
        }
    }
}