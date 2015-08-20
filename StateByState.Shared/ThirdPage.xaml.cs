using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BuiltToRoam.Lifecycle;


namespace StateByState
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThirdPage 
    {
        public ThirdPage()
        {
            InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var frame = Split.FindName("InnerFrame") as Frame;

            var fn = new FrameNavigation<ThirdStates, ThirdTransitions>(frame, CurrentViewModel);
            fn.Register<ThrirdOnePage>(ThirdStates.One);
            fn.Register<ThirdTwoPage>(ThirdStates.Two);
            fn.Register<ThirdThreePage>(ThirdStates.Three);
            fn.Register<ThirdFourPage>(ThirdStates.Four);
            await CurrentViewModel.Start();
        }

        public ThirdViewModel CurrentViewModel=>DataContext as ThirdViewModel;
    }
}
