using Godot;
using System;

namespace GrapplingHook.Physics
{
    public partial class HookStart : RigidBody2D
    {
        private bool _changePosition;
        private bool _controlsActivated;

        private Vector2 _position;

        public override void _IntegrateForces(PhysicsDirectBodyState2D state)
        {
            if (_changePosition)
            {
                state.Transform = new Transform2D(0, _position);
                _changePosition = false;
            }

            if (_controlsActivated)
            {
                var moveDirection = Input.GetAxis("Move_Left", "Move_Right");                

                var direction = GlobalPosition - (GetParent() as Node2D).GlobalPosition;
                var angle = Mathf.RadToDeg(direction.Angle());

                if (angle > 20 && angle < 160)
                {
                    state.ApplyImpulse(moveDirection * new Vector2(25, 0));
                }
            }
        }

        public void SetControlsActive(bool value)
        {
            _controlsActivated = value;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _changePosition = true;
        }
    }
}
