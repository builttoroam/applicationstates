using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BuiltToRoam.Lifecycle.States.ViewModel;

namespace BuiltToRoam.Lifecycle
{
    public class BasePage : Page
    {
        public INotifyPropertyChanged ViewModel => DataContext as INotifyPropertyChanged;

        public BasePage()
        {
            var mgr = Application.Current.Resources[typeof(CoreApplication).Name];
            if (mgr == null) return;

            var props = mgr.GetType().GetTypeInfo().DeclaredProperties.FirstOrDefault(p=>p.Name=="StateManager");
            var manager = props?.GetValue(mgr) as IHasCurrentViewModel;
            var dc= manager?.CurrentViewModel;
            DataContext = dc;
        }
    }
}