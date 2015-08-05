using Windows.UI.Xaml.Controls;

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
    }
}
