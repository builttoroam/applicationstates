using System;
using System.Threading.Tasks;

namespace BuiltToRoam.Lifecycle
{
    public interface IUIContext
    {
        Task RunOnUIThreadAsync(Func<Task> action);
    }
}