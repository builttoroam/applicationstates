using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class ViewModelStateDefinition<TState, TViewModel> :
        BaseStateDefinition<TState>,
        IViewModelStateDefinition<TState, TViewModel>
        where TState : struct
        where TViewModel : INotifyPropertyChanged //, new()
    {
        public Type ViewModelType => typeof(TViewModel);

        public async Task<INotifyPropertyChanged> Generate(IContainer container)
        {
            var vm = ServiceLocator.Current.GetInstance<TViewModel>();
            //var vm = new TViewModel();

            // Attempt to register any classes (ie nested view models) that may be required
            (vm as ICanRegisterDependencies)?.RegisterDependencies(container);

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
        public static IViewModelStateDefinition<TState, TViewModel> Initialise<TState, TViewModel>(
            this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
            Action<TViewModel> action)
                    where TState : struct
        where TViewModel : INotifyPropertyChanged

        {
#pragma warning disable 1998 // Convert sync method into async call
            return stateDefinition.Initialise(async vm => action(vm));
#pragma warning restore 1998
        }

        public static IViewModelStateDefinition<TState, TViewModel> Initialise<TState, TViewModel>(
            this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
            Func<TViewModel, Task> action)
                    where TState : struct
        where TViewModel : INotifyPropertyChanged

        {
            if (stateDefinition == null) return null;
            stateDefinition.InitialiseViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenAboutToChange<TState, TViewModel>(
    this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
    Action<TViewModel,CancelEventArgs> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
#pragma warning disable 1998 // Convert sync method into async call
            return stateDefinition.WhenAboutToChange(async (vm,cancel) => action(vm,cancel));
#pragma warning restore 1998
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenAboutToChange<TState, TViewModel>(
    this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
    Func<TViewModel, CancelEventArgs, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
            if (stateDefinition == null) return null;

            stateDefinition.AboutToChangeFromViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangingFrom<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Action<TViewModel> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
#pragma warning disable 1998  // Convert sync method into async call
            return stateDefinition.WhenChangingFrom(async vm => action(vm));
#pragma warning restore 1998
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangingFrom<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Func<TViewModel, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
            if (stateDefinition == null) return null;

            stateDefinition.ChangingFromViewModel = action;
            return stateDefinition;
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangedTo<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Action<TViewModel> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
#pragma warning disable 1998  // Convert sync method into async call
            return stateDefinition.WhenChangedTo(async vm=> action(vm));
#pragma warning restore 1998
        }

        public static IViewModelStateDefinition<TState, TViewModel> WhenChangedTo<TState, TViewModel>(
this IViewModelStateDefinition<TState, TViewModel> stateDefinition,
Func<TViewModel, Task> action) where TState : struct
        where TViewModel : INotifyPropertyChanged
        {
            if (stateDefinition == null) return null;

            stateDefinition.ChangedToViewModel = action;
            return stateDefinition;
        }

    }
}
