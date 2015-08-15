using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class ViewModelStateDefinition<TState, TViewModel> :
        BaseStateDefinition<TState>,
        IViewModelStateDefinition<TState, TViewModel>
        where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
    {
        public Type ViewModelType => typeof(TViewModel);

        public async Task<INotifyPropertyChanged> Generate()
        {
            var vm = new TViewModel();
            if (InitialiseViewModel != null)
            {
                await InitialiseViewModel(vm);
            }
            return vm;
        }

        public Func<TViewModel, Task> InitialiseViewModel { get; set; }

        public Func<TViewModel, CancelEventArgs, Task> AboutToChangeFromViewModel { get; set; }

        public async Task InvokeAboutToChangeFromViewModel(INotifyPropertyChanged viewModel, CancelEventArgs cancel)
        {
            if (AboutToChangeFromViewModel == null) return;
            await AboutToChangeFromViewModel((TViewModel)viewModel, cancel);
        }

        public Func<TViewModel, Task> ChangingFromViewModel { get; set; }

        public async Task InvokeChangingFromViewModel(INotifyPropertyChanged viewModel)
        {
            if (ChangingFromViewModel == null) return;
            await ChangingFromViewModel((TViewModel)viewModel);
        }


        public Func<TViewModel, Task> ChangedToViewModel { get; set; }

        public async Task InvokeChangedToViewModel(INotifyPropertyChanged viewModel)
        {
            if (ChangedToViewModel == null) return;
            await ChangedToViewModel((TViewModel)viewModel);
        }


    }

    public static class ViewModelStateHelper
    {
        //Func<TViewModel, Task> InitialiseViewModel { get; set; }

        //Func<TViewModel, CancelEventArgs, Task> AboutToChangeFromViewModel { get; set; }


        //Func<TViewModel, Task> ChangingFromViewModel { get; set; }

        //Func<TViewModel, Task> ChangedToViewModel { get; set; }

        public static IViewModelStateDefinition<TState, TViewModel> Initialise<TState, TViewModel>(
            this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
            Action<TViewModel> action)
                    where TState : struct
        where TViewModel : INotifyPropertyChanged, new()

        {
            return stateDefinition.Initialise(async vm => action(vm));
        }

        public static IViewModelStateDefinition<TState, TViewModel> Initialise<TState, TViewModel>(
            this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
            Func<TViewModel, Task> action)
                    where TState : struct
        where TViewModel : INotifyPropertyChanged, new()

        {
            if (stateDefinition == null) return stateDefinition;
            stateDefinition.InitialiseViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenAboutToChange<TState, TViewModel>(
    this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
    Action<TViewModel,CancelEventArgs> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            return stateDefinition.WhenAboutToChange(async (vm,cancel) => action(vm,cancel));
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenAboutToChange<TState, TViewModel>(
    this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
    Func<TViewModel, CancelEventArgs, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            if (stateDefinition == null) return stateDefinition;

            stateDefinition.AboutToChangeFromViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangingFrom<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Action<TViewModel> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            return stateDefinition.WhenChangingFrom(async vm => action(vm));
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangingFrom<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Func<TViewModel, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            if (stateDefinition == null) return stateDefinition;

            stateDefinition.ChangingFromViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangedTo<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Action<TViewModel> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            return stateDefinition.WhenChangedTo(async vm=> action(vm));
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangedTo<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Func<TViewModel, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
        {
            if (stateDefinition == null) return stateDefinition;

            stateDefinition.ChangedToViewModel = action;
            return stateDefinition;
        }

    }
}
