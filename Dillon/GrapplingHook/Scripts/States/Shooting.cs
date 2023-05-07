using Godot;
using PlayerController.States;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook will move to the target position.
    /// When it reached the target position the grappling hook changes his state to connected.
    /// </summary>
    public class Shooting : State
    {
        public override string Name { get { return "Shooting"; } }

        public Shooting(GrapplingHook grapplingHook) : base(grapplingHook) { }

        private Tween _tween;
        //private PhysicsPointQueryParameters2D _query;
        private Vector2 _targetPosition;
        private Vector2 _startPosition;

        public override void Enter()
        {
            _targetPosition = _grapplingHook.GetHookTargetPosition();
            _startPosition = _grapplingHook.GetHookStartPosition();

            // Set the start position and the rotation of the hook
            _grapplingHook.Hook.GlobalPosition = _startPosition;
            _grapplingHook.Hook.LookAt(_targetPosition);

            _tween = _grapplingHook.GetTree().CreateTween();
            _tween.TweenProperty(_grapplingHook.Hook, "position", _targetPosition, 0.1f);
            _tween.Finished += TransitionToConnected;
        }

        public override void Execute()
        {
            //uint collisionMask = 1;
            //collisionMask = collisionMask << 11;

            //var spaceState = _grapplingHook.Hook.GetWorld2D().DirectSpaceState;

            //if(_query == null)
            //{
            //    _query = new PhysicsPointQueryParameters2D();
            //    _query.Position = _targetPosition;
            //    _query.CollideWithBodies = false;
            //    _query.CollideWithAreas = true;
            //    _query.CollisionMask = collisionMask;
            //}
           
            //var result = spaceState.IntersectPoint(_query);

            //if (result.Count > 0 && _tween.IsRunning() == false)
            //{
            //    TransitionToConnected();
            //}
        }

        public override void Exit() { }

        private void TransitionToConnected()
        {
            _tween.Finished -= TransitionToConnected;
            _grapplingHook.ChangeState(new Connected(_grapplingHook));
        }
    }
}
