using System.ComponentModel;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle.States.ViewModel
{
    public interface IAboutToLeaveViewModelState
    {
        Task AboutToLeave(CancelEventArgs cancel);
    }
}