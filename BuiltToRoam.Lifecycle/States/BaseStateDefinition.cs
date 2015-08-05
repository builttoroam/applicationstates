namespace BuiltToRoam.Lifecycle.States
{
    public class BaseStateDefinition<TState> : IStateDefinition<TState>
        where TState : struct
    {
        public TState State { get; set; }
    }
}