using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States
{
    public interface IStateDefinition<TState> where TState : struct
    {
        TState State { get; }

        Func<CancelEventArgs, Task> AboutToChangeFrom { get; set; }
        Func<Task> ChangingFrom { get; set; }
        Func<Task> ChangedTo { get; set; }
    }

    public static class StateHelper
    {
        public static IStateDefinition<TState> WhenAboutToChange<TState>(
    this IStateDefinition<TState> stateDefinition,
    Action<CancelEventArgs> action) where TState : struct
        {
#pragma warning disable 1998  // Convert sync method into async call
            return stateDefinition.WhenAboutToChange(async cancel => action(cancel));
#pragma warning restore 1998
        }

        public static IStateDefinition<TState> WhenAboutToChange<TState>(
    this IStateDefinition<TState> stateDefinition,
    Func<CancelEventArgs, Task> action) where TState : struct
        {
            if (stateDefinition == null) return null;

                stateDefinition.AboutToChangeFrom = action;
            return stateDefinition;
        }

        public static IStateDefinition<TState> WhenChangingFrom<TState>(
this IStateDefinition<TState> stateDefinition,
Action action) where TState : struct
        {
#pragma warning disable 1998  // Convert sync method into async call
            return stateDefinition.WhenChangingFrom(async () => action());
#pragma warning restore 1998
        }

        public static IStateDefinition<TState> WhenChangingFrom<TState>(
this IStateDefinition<TState> stateDefinition,
Func<Task> action) where TState : struct
        {
            if (stateDefinition == null) return null;

            stateDefinition.ChangingFrom = action;
            return stateDefinition;
        }

        public static IStateDefinition<TState> WhenChangedTo<TState>(
this IStateDefinition<TState> stateDefinition,
Action action) where TState : struct
        {
#pragma warning disable 1998  // Convert sync method into async call
            return stateDefinition.WhenChangedTo(async () => action());
#pragma warning restore 1998
        }

        public static IStateDefinition<TState> WhenChangedTo<TState>(
this IStateDefinition<TState> stateDefinition,
Func<Task> action) where TState : struct
        {
            if (stateDefinition == null) return null;

            stateDefinition.ChangedTo= action;
            return stateDefinition;
        }


        public static ITransitionDefinition<TState> From<TState>(this ITransitionDefinition<TState> transition,
            TState state) where TState : struct
        {
            transition.StartState = state;
            return transition;
        }

        public static ITransitionDefinition<TState> To<TState>(this ITransitionDefinition<TState> transition,
            TState state) where TState : struct
        {
            transition.EndState= state;
            return transition;
        }

      
    }

}