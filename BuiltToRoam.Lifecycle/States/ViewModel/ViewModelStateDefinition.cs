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
    }
}