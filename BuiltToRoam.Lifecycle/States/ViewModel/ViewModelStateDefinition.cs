using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class ViewModelStateDefinition<TState, TViewModel> : BaseStateDefinition<TState>, IGenerateViewModel
        where TState : struct
        where TViewModel : INotifyPropertyChanged, new()
    {
        public Type ViewModelType => typeof (TViewModel);

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

    public interface ILeavingViewModelState
    {
        Task Leaving();
    }

    public interface IArrivingViewModelState
    {
        Task Arriving();
    }

    public interface IAboutToLeaveViewModelState
    {
        Task AboutToLeave(CancelEventArgs cancel);
    }



}