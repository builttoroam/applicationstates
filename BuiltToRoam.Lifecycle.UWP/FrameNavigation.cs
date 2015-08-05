using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BuiltToRoam.Lifecycle.States;

namespace BuiltToRoam.Lifecycle
{
    public class FrameNavigation<TState,TTransition> 
        where TState : struct
        where TTransition:struct
    {
        public IDictionary<TState, Type> NavigationIndex { get; } = new Dictionary<TState, Type>();

        public IStateManager<TState, TTransition> StateManager { get; }

        private Frame RootFrame { get; }

        public FrameNavigation(Frame rootFrame,
            IHasStateManager<TState, TTransition> hasStateManager,
            string registerAs = null)
        {
            var stateManager = hasStateManager.StateManager;
            if (registerAs == null)
            {
                registerAs = hasStateManager.GetType().Name;
            }
            Application.Current.Resources[registerAs] = this;
            RootFrame = rootFrame;
            RootFrame.Tag = registerAs;
            StateManager = stateManager;
            StateManager.StateChanged += StateManager_StateChanged;
        }

        private void StateManager_StateChanged(object sender, StateEventArgs<TState> e)
        {
            var tp = NavigationIndex[e.State];
            if (RootFrame.BackStack.FirstOrDefault()?.SourcePageType == tp)
            {
                RootFrame.GoBack();
            }
            else
            {
                RootFrame.Navigate(tp);
            }
        }

        public void Register<TPage>(TState state)
        {
            NavigationIndex[state] = typeof (TPage);
        }

    }
}