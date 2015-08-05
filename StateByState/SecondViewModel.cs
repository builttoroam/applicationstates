using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BuiltToRoam.Lifecycle;

namespace StateByState
{
    public enum SecondStates
    {
        Base,
        State1,
        State2,
        State3
    }

    public enum SecondStates2
    {
        Base,
        StateX,
        StateY,
        StateZ
    }
    public class SecondViewModel : NotifyBase
    {
        public event EventHandler SecondCompleted;

        public async Task InitSecond()
        {
            await Task.Delay(1000);
            Debug.WriteLine("Break");
        }

        public void GoBack()
        {
            SecondCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}