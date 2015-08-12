using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BuiltToRoam;
using BuiltToRoam.Lifecycle;

namespace StateByState
{
    public class MainViewModel : NotifyBase
    {
        public event EventHandler Completed;
        public event EventHandler UnableToComplete;

        public string Data { get; set; }

#pragma warning disable 1998 // So we can do async actions
        public async Task Init()
#pragma warning restore 1998
        {
            Data = "Hello Page 1";
            Debug.WriteLine("Break");
        }

        public void Test()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public void Three()
        {
            UnableToComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}