﻿using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StateByState
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecondPage
    {
        public SecondPage()
        {
            this.InitializeComponent();
        }

        public SecondViewModel CurrentViewModel =>DataContext as SecondViewModel;


        private void ToFirstClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (DataContext as SecondViewModel).ToFirst();
        }
        private void ToSecondClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (DataContext as SecondViewModel).ToSecond();

        }
        private void ToThirdClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (DataContext as SecondViewModel).ToThird();

        }

        private void DoneClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (DataContext as SecondViewModel).Done();
        }

        private async void XtoZ(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await CurrentViewModel.XtoZ();
        }

        private async void YtoZ(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await CurrentViewModel.XtoZ();
        }

        private async void ZtoY(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await CurrentViewModel.XtoZ();
        }

        private async void YtoX(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await CurrentViewModel.XtoZ();
        }
    }
}