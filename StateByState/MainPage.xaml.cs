﻿using System.Collections.Specialized;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StateByState
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TestClick(object sender, RoutedEventArgs e)
        {
            (ViewModel as MainViewModel)?.Test();
        }

        private void ThreeClick(object sender, RoutedEventArgs e)
        {
            (ViewModel as MainViewModel)?.Three();
        }
    }
    
}
