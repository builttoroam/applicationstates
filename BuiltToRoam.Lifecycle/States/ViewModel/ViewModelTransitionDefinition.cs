using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class ViewModelTransitionDefinition<TState> : BaseTransitionDefinition<TState>, IViewModelTransitionDefinition<TState>
        where TState : struct
    {
        public Func<TState, INotifyPropertyChanged, CancelEventArgs, Task> LeavingStateViewModel { get; set; }

        public Func<TState, INotifyPropertyChanged, Task> ArrivedStateViewModel { get; set; }
    }
}