using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface ICanRegisterDependencies
    {
        void RegisterDependencies(IContainer container);
    }


    public interface IViewModelStateManager<TState, TTransition> :
        ICanRegisterDependencies,
        IStateManager<TState, TTransition>,
        IHasCurrentViewModel
        where TState : struct
        where TTransition : struct
    {
        IViewModelStateDefinition<TState, TViewModel> DefineViewModelState<TViewModel>(TState state)
            where TViewModel : INotifyPropertyChanged;//, new();

        IViewModelStateDefinition<TState, TViewModel> DefineViewModelState<TViewModel>(
            IViewModelStateDefinition<TState, TViewModel> stateDefinition)
            where TViewModel : INotifyPropertyChanged;//, new();

        

    }

    public class ViewModelStateManager<TState, TTransition> : BaseStateManager<TState, TTransition>, IViewModelStateManager<TState,TTransition>
        where TState : struct
        where TTransition : struct
    {
        private const string ErrorDontUseDefineState =
            "Use DefineViewModelState instead of DefineState for ViewModelStateManager";
        public override IStateDefinition<TState> DefineState(TState state)
        {
            throw new Exception(ErrorDontUseDefineState);
        }

        public override IStateDefinition<TState> DefineState(IStateDefinition<TState> stateDefinition)
        {
            if (stateDefinition.GetType().GetGenericTypeDefinition() != typeof(ViewModelStateDefinition<,>)) throw new Exception(ErrorDontUseDefineState);
            return base.DefineState(stateDefinition);
        }

        public IViewModelStateDefinition<TState,TViewModel> DefineViewModelState<TViewModel>(TState state) where TViewModel : INotifyPropertyChanged//, new()
        {
            var stateDefinition = new ViewModelStateDefinition<TState, TViewModel> { State = state };
            return DefineViewModelState(stateDefinition);
        }

        public IViewModelStateDefinition<TState,TViewModel> DefineViewModelState<TViewModel>(IViewModelStateDefinition<TState, TViewModel> stateDefinition) where TViewModel : INotifyPropertyChanged//, new()
        {
            base.DefineState(stateDefinition);
            return stateDefinition;
        }

        public override ITransitionDefinition<TState> DefineTransition(TTransition transition)
        {
            var transitionDefinition = new ViewModelTransitionDefinition<TState>();
            Transitions[transition] = transitionDefinition;
            return transitionDefinition;
        }

        protected override ITransitionDefinition<TState> CreateDefaultTransition()
        {
            return new ViewModelTransitionDefinition<TState>();
        }

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
                await leaving.Leaving();
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
                vm = await newGen.Generate(DependencyContainer);
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
            if (trans != null && trans.ArrivedStateViewModel != null)
            {
                await trans.ArrivedStateViewModel(currentState, CurrentViewModel);
            }
        }

        protected async override Task LeavingState(ITransitionDefinition<TState> transition, TState currentState, CancelEventArgs cancel)
        {
            await base.LeavingState(transition, currentState, cancel);

            if (cancel.Cancel) return;

            var trans = transition as ViewModelTransitionDefinition<TState>;
            if (trans != null && trans.LeavingStateViewModel != null)
            {
                await trans.LeavingStateViewModel(currentState, CurrentViewModel, cancel);
            }

        }

        protected IContainer DependencyContainer { get; private set; }
        public void RegisterDependencies(IContainer container)
        {
            DependencyContainer = container;
            var cb = new ContainerBuilder();
            foreach (var state in States.Values.OfType<IGenerateViewModel>())
            {
                cb.RegisterType(state.ViewModelType);
            }
            cb.Update(container);
        }
    }
}