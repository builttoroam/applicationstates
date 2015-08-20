using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;

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

        public void DefineAllStates()
        {
            var vals = Enum.GetValues(typeof (TState));
            foreach (var enumVal in vals)
            {
                DefineState((TState) enumVal);
            }
        }

        public virtual IStateDefinition<TState> DefineState(TState state)
        {
            var stateDefinition = new BaseStateDefinition<TState> { State = state };
            return DefineState(stateDefinition);
        }

        public virtual IStateDefinition<TState> DefineState(IStateDefinition<TState> stateDefinition)
        {
            Debug.WriteLine("Defining state of type " + stateDefinition.GetType().Name);
            States[stateDefinition.State] = stateDefinition;
            return stateDefinition;
        }

        public virtual ITransitionDefinition<TState> DefineTransition(TTransition transition)
        {
            var transitionDefinition = new BaseTransitionDefinition<TState>();
            Transitions[transition] = transitionDefinition;
            Debug.WriteLine("Defining transition of type " + transition.GetType().Name);
            return transitionDefinition;
        }

        public async Task<bool> ChangeTo(TState newState, bool useTransitions = true)
        {
            Debug.WriteLine($"Changing to state {newState} ({useTransitions})");
            var current = CurrentState;
            if (current.Equals(newState))
            {
                Debug.WriteLine("Transitioning to same state - doing nothing");
                return true;
            }


            if (!current.Equals(default(TState)))
            {
                Debug.WriteLine("Current state is " + current);
                var currentStateDef = States[current];
                var cancel = new CancelEventArgs();
                if (currentStateDef.AboutToChangeFrom != null)
                {
                    Debug.WriteLine("Invoking 'AboutToChangeFrom' on current state definition");
                    await currentStateDef.AboutToChangeFrom(cancel);
                    Debug.WriteLine("'AboutToChangeFrom' completed");
                }
                if (cancel.Cancel)
                {
                    Debug.WriteLine("Cancelling state transition invoking 'AboutToChangeFrom'");
                    return false;
                }
            }


            try
            {
                Debug.WriteLine("Invoking internal ChangeToState to perform state change");
                var proceed = await ChangeToState(current, newState);
                if (!proceed)
                {
                    Debug.WriteLine("Unable to complete ChangeToState so exiting the ChangeTo, returning false");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }

            Debug.WriteLine($"About to updated CurrentState (currently: {CurrentState})");
            CurrentState = newState;
            Debug.WriteLine($"CurrentState updated (now: {CurrentState})");

            var newStateDef = States.SafeDictionaryValue<TState,IStateDefinition<TState>, IStateDefinition<TState>>(newState);
            if (newStateDef.ChangedTo != null)
            {
                Debug.WriteLine($"State definition found, of type {newStateDef.GetType().Name}, invoking ChangedTo method");
                await newStateDef.ChangedTo();
                Debug.WriteLine("ChangedTo completed");
            }
            else
            {
                Debug.WriteLine("No new state definition");
            }

            try
            {
                if (StateChanged != null)
                {
                    Debug.WriteLine("Invoking StateChanged event");
                    StateChanged.Invoke(this, new StateEventArgs<TState>(newState, useTransitions));
                    Debug.WriteLine("StateChanged event completed");
                }
                else
                {
                    Debug.WriteLine("Nothing listening to StateChanged");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:" + ex.Message);
                // Ignore any errors caused by the event being raised, as 
                // the state change has still occurred
            }

            Debug.WriteLine("ChangeTo completed");
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