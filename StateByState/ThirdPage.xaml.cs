using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BuiltToRoam.Lifecycle;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StateByState
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThirdPage 
    {
        public ThirdPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var fn = new FrameNavigation<ThirdStates, ThirdTransitions>(this.InnerFrame, CurrentViewModel);
            fn.Register<ThrirdOnePage>(ThirdStates.One);
            fn.Register<ThirdTwoPage>(ThirdStates.Two);
            fn.Register<ThirdThreePage>(ThirdStates.Three);
            fn.Register<ThirdFourPage>(ThirdStates.Four);
        }

        public ThirdViewModel CurrentViewModel=>DataContext as ThirdViewModel;

        private async void LoadOneClick(object sender, RoutedEventArgs e)
        {
            await CurrentViewModel.One();
        }
        private async void LoadTwoClick(object sender, RoutedEventArgs e)
        {
            await CurrentViewModel.Two();
        }
        private async void LoadThreeClick(object sender, RoutedEventArgs e)
        {
            await CurrentViewModel.Three();
        }
        private async void LoadFourClick(object sender, RoutedEventArgs e)
        {
            await CurrentViewModel.Four();
        }
    }
}
