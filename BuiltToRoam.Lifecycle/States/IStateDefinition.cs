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
            return stateDefinition.WhenAboutToChange(async cancel => action(cancel));
        }

        public static IStateDefinition<TState> WhenAboutToChange<TState>(
    this IStateDefinition<TState> stateDefinition,
    Func<CancelEventArgs, Task> action) where TState : struct
        {
            if (stateDefinition == null) return stateDefinition;

                stateDefinition.AboutToChangeFrom = action;
            return stateDefinition;
        }

        public static IStateDefinition<TState> WhenChangingFrom<TState>(
this IStateDefinition<TState> stateDefinition,
Action action) where TState : struct
        {
            return stateDefinition.WhenChangingFrom(async () => action());
        }

        public static IStateDefinition<TState> WhenChangingFrom<TState>(
this IStateDefinition<TState> stateDefinition,
Func<Task> action) where TState : struct
        {
            if (stateDefinition == null) return stateDefinition;

            stateDefinition.ChangingFrom = action;
            return stateDefinition;
        }

        public static IStateDefinition<TState> WhenChangedTo<TState>(
this IStateDefinition<TState> stateDefinition,
Action action) where TState : struct
        {
            return stateDefinition.WhenChangedTo(async () => action());
        }

        public static IStateDefinition<TState> WhenChangedTo<TState>(
this IStateDefinition<TState> stateDefinition,
Func<Task> action) where TState : struct
        {
            if (stateDefinition == null) return stateDefinition;

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

        //public static IStateDefinition<TState> WithBehavior<TState>(
        //    this IStateDefinition<TState> stateDefinition,
        //    StateCancellableAction actionType,
        //    Func<CancelEventArgs, Task> action) where TState : struct
        //{
        //    if (stateDefinition == null) return stateDefinition;

        //    if (actionType == StateCancellableAction.AboutToChange)
        //    {
        //        stateDefinition.AboutToChangeFrom = action;
        //    }
        //    return stateDefinition;
        //}

        //public static IStateDefinition<TState> WithBehavior<TState>(
        //    this IStateDefinition<TState> stateDefinition,
        //    StateAction actionType,
        //    Func<Task> action) where TState:struct
        //{
        //    if (stateDefinition == null) return stateDefinition;
        //    switch (actionType)
        //    {
        //        case StateAction.AboutToChange:
        //            stateDefinition.AboutToChangeFrom = (cancel) => action();
        //            break;
        //        case StateAction.ChangingFrom:
        //            stateDefinition.ChangingFrom = action;
        //            break;
        //            case StateAction.ChangedTo:
        //            stateDefinition.ChangedTo = action;
        //            break;
        //    }
        //    return stateDefinition;
        //} 
    }

}