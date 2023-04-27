using Godot;
using System;

namespace GrapplingHook.States
{
    public class Shooting : State
    {
        public override string Name { get { return "Shooting"; } }

        public Shooting(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter()
        {
            // Set the start position and the rotation of the hook
            _grapplingHook.Transform = new Transform2D(0, _grapplingHook.HookStartPosition);
            _grapplingHook.LookAt(_grapplingHook._globalTargetPosition);
            // Set the direction
            _grapplingHook._direction = (_grapplingHook._globalTargetPosition - _grapplingHook.HookStartPosition)
                                         .Normalized();
        }

        public override void Execute()
        {
            var position = _grapplingHook.Transform.Origin;
            position += _grapplingHook._direction * _grapplingHook.HookSpeed * (float)_grapplingHook.GetProcessDeltaTime();
            _grapplingHook.Transform = new Transform2D(_grapplingHook.Rotation, position);

            CheckTransitionToConnected();
            CheckTransitionToReturning();
        }

        public override void Exit() { }

        private void CheckTransitionToConnected()
        {
            var collidingRay = _grapplingHook.rayCast2Ds.Find(o => o.IsColliding());
            if (collidingRay != null)
            {
                _grapplingHook.ChangeState(new Connected(_grapplingHook));
            }
        }
        private void CheckTransitionToReturning()
        {
            if (_grapplingHook.GetCurrentHookLenght() > _grapplingHook.HookLenght)
            {
                _grapplingHook.ChangeState(new Returning(_grapplingHook));
            }
        }

    }
}
