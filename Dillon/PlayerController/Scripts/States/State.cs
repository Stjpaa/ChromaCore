using Godot;
using System;

namespace PlayerController.States
{
    public abstract class State
    {
        public abstract string Name { get; }

        protected PlayerController2D _playerController2D;

        public State(PlayerController2D playerController2D)
        {
            _playerController2D = playerController2D;
        }
        public abstract void Enter();
        public virtual void ExecutePhysicsProcess(double delta) { }
        public virtual void ExecuteProcess() { }
        public abstract void Exit();
    }
}
