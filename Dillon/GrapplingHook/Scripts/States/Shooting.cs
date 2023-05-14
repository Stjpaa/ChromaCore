using Godot;
using PlayerController.States;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook will move to the target position.
    /// Therefore its rotation, start position and the target position is set.
    /// When it reached the target position the grappling hook changes his state to connected.
    /// </summary>
    public class Shooting : State
    {
        public override string Name { get { return "Shooting"; } }

        public Shooting(GrapplingHook grapplingHook) : base(grapplingHook) { }

        private Tween _tween;
        private Vector2 _targetPosition;
        private Vector2 _startPosition;

        public override void Enter()
        {
            _targetPosition = _grapplingHook.GetHookTargetPosition();
            _startPosition = _grapplingHook.GetHookStartPosition();

            // Set the start position and the rotation of the hook
            _grapplingHook.Hook.GlobalPosition = _startPosition;
            _grapplingHook.Hook.LookAt(_targetPosition);

            // Shooting animation of the grappling hook
            _tween = _grapplingHook.GetTree().CreateTween();
            _tween.TweenProperty(_grapplingHook.Hook, "position", _targetPosition, 0.1f);
            _tween.Finished += TransitionToConnected;
        }

        public override void ExecuteProcess() { }
        public override void ExecutePhysicsProcess() { }
        public override void Exit() { }

        private void TransitionToConnected()
        {
            _tween.Finished -= TransitionToConnected;
            _grapplingHook.ChangeState(new Connected(_grapplingHook));
        }
    }
}
