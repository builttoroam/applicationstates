﻿using System;
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
}