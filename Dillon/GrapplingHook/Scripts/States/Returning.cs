using Godot;
using System;

namespace GrapplingHook.States
{
    public class Returning : State
    {
        public override string Name { get { return "Returning"; } }

        public Returning(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter() { }

        public override void Execute()
        {
            _grapplingHook._direction = (_grapplingHook.HookStartPosition - _grapplingHook.GlobalPosition)
                                         .Normalized();

            var position = _grapplingHook.Transform.Origin;
            position += _grapplingHook._direction * _grapplingHook.HookSpeed * (float)_grapplingHook.GetProcessDeltaTime();
            _grapplingHook.Transform = new Transform2D(_grapplingHook.Rotation, position);

            CheckTransitionToIdle();
        }

        public override void Exit() { }

        private void CheckTransitionToIdle()
        {
            if (_grapplingHook.GetCurrentHookLenght() < 5f)
            {
                _grapplingHook.ChangeState(new Idle(_grapplingHook));
                _grapplingHook.PlayerController.ChangeState(_grapplingHook.PlayerController.previousState);
            }
        }
    }
}
