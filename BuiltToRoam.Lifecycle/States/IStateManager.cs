using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States
{
    public interface IStateManager<TState, TTransition> : INotifyPropertyChanged
        where TState : struct
        where TTransition:struct
    {
        event EventHandler<StateEventArgs<TState>> StateChanged;

        TState CurrentState { get; }

        IDictionary<TState, IStateDefinition<TState>> States { get; }

        IDictionary<TTransition, ITransitionDefinition<TState>> Transitions { get; }

        Task<bool> ChangeTo(TState newState, bool useTransition = true);

        Task<bool> Transition(TTransition transition, bool useTransition = true);
    }
}