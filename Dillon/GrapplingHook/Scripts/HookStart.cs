using Godot;
using System;

namespace GrapplingHook.Physics
{
    public partial class HookStart : RigidBody2D
    {
        private bool _setPosition;
        private bool _controlsActivated;
        private bool _setVelocity;

        private Vector2 _position;
        private Vector2 _velocity;

        public override void _IntegrateForces(PhysicsDirectBodyState2D state)
        {
            if (_setPosition)
            {
                state.Transform = new Transform2D(0, _position);
                _setPosition = false;
            }

            if(_setVelocity)
            {
                state.LinearVelocity = _velocity;
                _setVelocity = false;
            }

            if (_controlsActivated)
            {
                var moveDirection = Input.GetAxis("Move_Left", "Move_Right");                

                var direction = GlobalPosition - (GetParent() as Node2D).GlobalPosition;
                var angle = Mathf.RadToDeg(direction.Angle());

                if (angle > 20 && angle < 160)
                {
                    state.ApplyImpulse(moveDirection * new Vector2(700, 0) * (float)GetPhysicsProcessDeltaTime());
                }
                else
                {
                    state.ApplyImpulse( -LinearVelocity * 3 * (float)GetPhysicsProcessDeltaTime());
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _setPosition = true;
            LinearVelocity = Vector2.Zero;
        }

        public void SetStartVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            _setVelocity = true;
        }

        public void SetControlsActive(bool value)
        {
            _controlsActivated = value;
            _setVelocity = true;
        }
    }
}
