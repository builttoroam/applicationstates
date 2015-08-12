using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface IGenerateViewModel
    {
        Type ViewModelType { get; }
        Task<INotifyPropertyChanged> Generate();

        Task InvokeAboutToChangeFromViewModel(INotifyPropertyChanged viewModel, CancelEventArgs cancel);

        Task InvokeChangingFromViewModel(INotifyPropertyChanged viewModel);

        Task InvokeChangedToViewModel(INotifyPropertyChanged viewModel);

    }
}