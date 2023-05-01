using Godot;
using System;

namespace GrapplingHook.States
{
    public class Idle : State
    {
        public override string Name { get { return "Idle"; } }

        public Idle(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter()
        {
            _grapplingHook.Visible = false;
            _grapplingHook.SetProcess(false);
            _grapplingHook.SetPhysicsProcess(false);
            _grapplingHook.shootEvent += TransitionToMooving;
        }

        public override void Execute() { }

        public override void Exit()
        {
            _grapplingHook.Visible = true;
            _grapplingHook.SetProcess(true);
            _grapplingHook.SetPhysicsProcess(true);
            _grapplingHook.shootEvent -= TransitionToMooving;
        }

        private void TransitionToMooving()
        {
            _grapplingHook.ChangeState(new Shooting(_grapplingHook));
        }
    }
}
