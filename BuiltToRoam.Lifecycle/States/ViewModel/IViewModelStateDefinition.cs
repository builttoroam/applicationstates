using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface IViewModelStateDefinition<TState,TViewModel> :IGenerateViewModel, IStateDefinition<TState> 
        where TViewModel: INotifyPropertyChanged 
        where TState : struct
    {
        Func<TViewModel, Task> InitialiseViewModel { get; set; }

        Func<TViewModel, CancelEventArgs, Task> AboutToChangeFromViewModel { get; set; }

    
        Func<TViewModel, Task> ChangingFromViewModel { get; set; }

        Func<TViewModel, Task> ChangedToViewModel { get; set; }

    }
}