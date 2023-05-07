
using Godot;

namespace GrapplingHook.States
{
    public abstract class State
    {
        public abstract string Name { get; }

        protected GrapplingHook _grapplingHook;

        public State(GrapplingHook grapplingHook)
        {
            _grapplingHook = grapplingHook;
        }
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}
