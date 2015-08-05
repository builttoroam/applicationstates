using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface IGenerateViewModel
    {
        Type ViewModelType { get; }
        Task<INotifyPropertyChanged> Generate();
    }
}