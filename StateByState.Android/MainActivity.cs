using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Autofac;
using BuiltToRoam.Lifecycle.States;

namespace StateByState.Android
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon")]
    public class StartActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var core = new CoreApplication();
            var fn = new AcitivityNavigation<PageStates, PageTransitions>(this, core);
            fn.Register<MainActivity>(PageStates.Main);
            fn.Register<SecondActivity>(PageStates.Second);
            //fn.Register<ThirdActivity>(PageStates.Third);
            await core.Startup(builder =>
            {
                builder.RegisterType<Special>().As<ISpecial>();
            });

        }
    }


    [Activity(Label = "@string/FirstTitle", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;


        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

          


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var  button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                //button.Text = string.Format("{0} clicks!", count++);
                var intent = new Intent(this, typeof (SecondActivity));
                StartActivity(intent);
            };
        }
    }


    [Activity(Label = "@string/SecondTitle")]
    public class SecondActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Second);

        }
    }



    public class AcitivityNavigation<TState, TTransition>
       where TState : struct
       where TTransition : struct
    {
        public IDictionary<TState, Type> NavigationIndex { get; } = new Dictionary<TState, Type>();

        public IStateManager<TState, TTransition> StateManager { get; }

        private Activity RootActivity { get; }

        public AcitivityNavigation(Activity rootActivity,
            IHasStateManager<TState, TTransition> hasStateManager,
            string registerAs = null)
        {
            var stateManager = hasStateManager.StateManager;
            if (registerAs == null)
            {
                registerAs = hasStateManager.GetType().Name;
            }
            //Application.Current.Resources[registerAs] = this;
            RootActivity = rootActivity;
            //RootActivity.Tag = registerAs;
            StateManager = stateManager;
            StateManager.StateChanged += StateManager_StateChanged;
        }

        private void StateManager_StateChanged(object sender, StateEventArgs<TState> e)
        {
            var tp = NavigationIndex[e.State];
            var intent = new Intent(RootActivity, tp);
            RootActivity.StartActivity(intent);
            //if (RootActivity.BackStack.FirstOrDefault()?.SourcePageType == tp)
            //{
            //    RootActivity.GoBack();
            //}
            //else
            //{
            //    RootActivity.Navigate(tp);
            //}
        }

        public void Register<TPage>(TState state)
        {
            NavigationIndex[state] = typeof(TPage);
        }

    }
}

