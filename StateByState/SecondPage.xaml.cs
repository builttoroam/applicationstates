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

        private void BackClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (ViewModel as SecondViewModel).GoBack();
        }
    }
}
