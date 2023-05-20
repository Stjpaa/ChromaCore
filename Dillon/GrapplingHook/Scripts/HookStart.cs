using Godot;
using System;

namespace GrapplingHook.Physics
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HookStart : RigidBody2D
    {
        private GrapplingHook_Data grapplingHookData;

        private bool _setPosition;
        private bool _controlsActivated;
        private bool _setVelocity;
        private bool _setStartImpulse;

        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _startImpulse;

        public override void _Ready()
        {
            var grapplingHook = (GetParent().GetParent().GetParent()) as GrapplingHook;
            grapplingHookData = grapplingHook.data;
        }

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

                // Player can cotrol the direction
                if (angle >= 20 && angle <= 160)
                {
                    state.ApplyImpulse(moveDirection * new Vector2(grapplingHookData.MoveSpeed, 0) * (float)GetPhysicsProcessDeltaTime());                   
                }
                else
                {
                    state.ApplyImpulse(grapplingHookData.CounterMoveSpeed * Vector2.Down * (float)GetPhysicsProcessDeltaTime());
                }

                if(_setStartImpulse)
                {
                    state.ApplyImpulse(_startImpulse);
                    _setStartImpulse = false;
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

        public void SetStartImpulse(Vector2 impulse)
        {
            _startImpulse = impulse;
            _setStartImpulse= true;
        }

        public void SetControlsActive(bool value)
        {
            _controlsActivated = value;
            _setVelocity = true;
        }
    }
}
