using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public class ViewModelStateManager<TState, TTransition> : BaseStateManager<TState, TTransition>, IHasCurrentViewModel
        where TState : struct
        where TTransition:struct
    {
        private IDictionary<Type, INotifyPropertyChanged> ViewModels { get; } =
            new Dictionary<Type, INotifyPropertyChanged>();

        public INotifyPropertyChanged CurrentViewModel { get; set; }

        public INotifyPropertyChanged Existing(Type viewModelType)
        {
            if (viewModelType == null) return null;

            INotifyPropertyChanged existing;
            ViewModels.TryGetValue(viewModelType, out existing);
            return existing;
        }


        protected override async Task<bool> ChangeToState(TState oldState, TState newState)
        {
            var aboutVM = CurrentViewModel as IAboutToLeaveViewModelState;
            var cancel = new CancelEventArgs();
            if (aboutVM != null)
            {
                
                await aboutVM.AboutToLeave(cancel);
                if (cancel.Cancel) return false;
            }

            var currentVMStates = !oldState.Equals(default(TState)) ? States[oldState] as IGenerateViewModel : null;
            if (currentVMStates != null)
            {
                await currentVMStates.InvokeAboutToChangeFromViewModel(CurrentViewModel, cancel);
                if (cancel.Cancel) return false;
            }

            var leaving = CurrentViewModel as ILeavingViewModelState;
            if (leaving != null)
            {
                await  leaving.Leaving();
            }

            if (currentVMStates != null)
            {
                await currentVMStates.InvokeChangingFromViewModel(CurrentViewModel);
            }

            var ok = await base.ChangeToState(oldState, newState);
            if (!ok) return false;
            INotifyPropertyChanged vm = null;
            if (!newState.Equals(default(TState)))
            {
                var current = States[newState] as IGenerateViewModel;
                var genType = current?.ViewModelType;

                vm = Existing(genType);
            }

            if (vm == null)
            {
                var newGen = States[newState] as IGenerateViewModel;
                if (newGen == null) return false;
                vm = await newGen.Generate();
            }
            if (vm == null) return false;

            ViewModels[vm.GetType()] = vm;
            CurrentViewModel = vm;

            var arrived = vm as IArrivingViewModelState;
            if (arrived != null)
            {
                await arrived.Arriving();
            }


            currentVMStates = States[newState] as IGenerateViewModel;
            if (currentVMStates != null)
            {
                await currentVMStates.InvokeChangedToViewModel(CurrentViewModel);
            }

            return true;
        }

        protected async override Task ArrivedState(ITransitionDefinition<TState> transition, TState currentState)
        {
            await base.ArrivedState(transition, currentState);

            var trans = transition as ViewModelTransitionDefinition<TState>;
            if (trans != null && trans.ArrivedStateViewModel!=null)
            {
                await trans.ArrivedStateViewModel(currentState, CurrentViewModel);
            }
        }

        protected async override Task LeavingState(ITransitionDefinition<TState> transition, TState currentState, CancelEventArgs cancel)
        {
            await base.LeavingState(transition, currentState, cancel);

            if (cancel.Cancel) return;

            var trans = transition as ViewModelTransitionDefinition<TState>;
            if (trans != null && trans.LeavingStateViewModel!=null)
            {
                await trans.LeavingStateViewModel(currentState, CurrentViewModel, cancel);
            }

        }


    }
}