using System.ComponentModel;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface IHasCurrentViewModel
    {
        INotifyPropertyChanged CurrentViewModel { get; }
    }
}