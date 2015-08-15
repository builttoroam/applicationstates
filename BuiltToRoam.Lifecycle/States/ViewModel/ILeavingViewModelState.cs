using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface ILeavingViewModelState
    {
        Task Leaving();
    }
}