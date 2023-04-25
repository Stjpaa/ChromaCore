using Godot;
using System;

namespace PlayerController.States
{
    public class Falling : State
    {
        public override string Name { get { return "Falling"; } }

        private float _apexGravityModifier;
        private float _apexMovementModifier;
        private float _apex;
        private float _timer;

        private bool _jumpBuffering;
        private bool _applyVariableJumpHeight;
        private bool _applyCoyoteTime;
        private bool _applyCustomGravity;

        private ApexModifierState _state;

        private Vector2 _customGravity;


        /// <summary>
        /// Falling state with default gravity and mechanics.
        /// </summary>
        public Falling(PlayerController2D playerController_2D, bool applyVariableJumpHeight, bool applyCoyoteTime) : base(playerController_2D)
        {
            _applyVariableJumpHeight = applyVariableJumpHeight;
            _applyCoyoteTime = applyCoyoteTime;
            _applyCustomGravity = false;
        }
        /// <summary>
        /// Falling state with default gravity and without mechanics.
        /// </summary>
        public Falling(PlayerController2D playerController_2D) : base(playerController_2D)
        {
            _applyCoyoteTime = false;
            _applyVariableJumpHeight = false;
            _applyCustomGravity = false;
        }
        /// <summary>
        /// Falling state without mechanics and custom gravity.
        /// </summary>
        public Falling(PlayerController2D playerController_2D, Vector2 customGravity) : base(playerController_2D)
        {
            _applyCoyoteTime = false;
            _applyVariableJumpHeight = false;
            _applyCustomGravity = true;
            _customGravity = customGravity;
        }

        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("InAir");
            _apexGravityModifier = 1f;
            _apexMovementModifier = 1f;
            _state = ApexModifierState.CanBeApplied;
        }
        public override void Execute()
        {
            if (ApplyCoyoteTime((float)_playerController2D.GetProcessDeltaTime())) { return; }

            ApplyMovement();

            if (_state == ApexModifierState.CanBeApplied)
            {
                UpdateApexModifier();
            }

            if (_applyCustomGravity) { ApplyCustomGravity(); }
            else { ApplyGravity(); }

            UpdateJumpBuffering();

            if (CheckTransitionToDash()) { return; }
            if (CheckTransitionToJumping()) { return; }
            if (CheckTransitionToHooking()) { return; }
            if (CheckTransitionToIdle()) { return; }
        }
        public override void Exit()
        {
            if (_playerController2D.IsOnFloor())
            {
                _playerController2D.Velocity = Vector2.Zero;
            }
        }

        private void ApplyMovement()
        {
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            var velocity = _playerController2D.Velocity;

            velocity.X += _playerController2D.FallingMoveSpeedAcceleration * _apexMovementModifier * moveDirection;

            if (_state is not ApexModifierState.BeingApplied)
            {
                velocity.X = Mathf.Clamp(velocity.X, -_playerController2D.MaxFallingMoveSpeed,
                                                      _playerController2D.MaxFallingMoveSpeed);
            }

            _playerController2D.Velocity = velocity;
        }
        private void ApplyGravity()
        {
            var jumpEndTrigger = Input.IsActionPressed("Jump");
            var velocity = _playerController2D.Velocity;

            // Variable jump height -> Extra fall speed if the jump button is not pressed
            if (jumpEndTrigger == false && _applyVariableJumpHeight)
            {
                velocity.Y += _playerController2D.FallSpeedAcceleration * _playerController2D.JumpEndModifier;
            }
            else
            {
                velocity.Y += _playerController2D.FallSpeedAcceleration;
            }

            velocity.Y = Mathf.Clamp(velocity.Y, float.MinValue, _playerController2D.MaxFallSpeed);

            velocity.Y *= _apexGravityModifier;
            _playerController2D.Velocity = velocity;
        }
        private void ApplyCustomGravity()
        {
            var velocity = _playerController2D.Velocity;
            velocity.Y += _customGravity.Y * 0.1f;
            velocity.X += _customGravity.X;
            _playerController2D.Velocity = velocity;
        }

        /// <summary>
        /// Short time period where the player is able to jump in air
        /// </summary>
        private bool ApplyCoyoteTime(float delta)
        {
            _timer += delta;
            if (_timer > _playerController2D.CoyoteTimeDuration && _applyCoyoteTime) { return false; }

            if (Input.IsActionJustPressed("Jump") && _playerController2D.previousState is Moving)
            {
                var velocity = _playerController2D.Velocity;
                velocity.Y = 0;
                _playerController2D.Velocity = velocity;

                _playerController2D.ChangeState(new Jumping(_playerController2D, true));
                return true;
            }
            return false;
        }

        private void UpdateApexModifier()
        {
            var velocity = _playerController2D.Velocity;
            if (_apex > velocity.Y && _playerController2D.previousState is Jumping)
            {
                _apex = velocity.Y;
            }
            else if (_apex <= velocity.Y && _apex < 0 && _state is ApexModifierState.CanBeApplied)
            {
                if (velocity.Y >= 0)
                {
                    // Apex modifier will be applied when the player reaches the highes position during the jump
                    // This is when his velocity is >= 0
                    ApplyApexModifier();
                }
            }
        }
        /// <summary>
        /// For a short time period the gravity is turned off and movement gets a boost
        /// </summary>
        private async void ApplyApexModifier()
        {
            _state = ApexModifierState.BeingApplied;
            _apexGravityModifier = 0f;
            _apexMovementModifier = _playerController2D.ApexModifierMovementBoost;

            await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(_playerController2D.ApexModifierDuration), SceneTreeTimer.SignalName.Timeout);

            _state = ApexModifierState.WasApplied;
            _apexGravityModifier = 1f;
            _apexMovementModifier = 1f;
        }

        private void UpdateJumpBuffering()
        {
            if (_jumpBuffering == false)
            {
                _jumpBuffering = Input.IsActionJustPressed("Jump");
            }
        }

        #region Transitions
        private bool CheckTransitionToIdle()
        {
            if (_playerController2D.IsOnFloor() && _applyCustomGravity == false)
            {
                _playerController2D.ChangeState(new Idle(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDash()
        {
            var dashTrigger = Input.IsActionJustPressed("Dash");
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            if (dashTrigger && moveDirection != 0 && Dashing._availableDashes != 0)
            {
                _playerController2D.ChangeState(new Dashing(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToJumping()
        {
            if (_jumpBuffering && _playerController2D.IsOnFloor())
            {
                _playerController2D.ChangeState(new Jumping(_playerController2D, true));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToHooking()
        {
            var hookTrigger = Input.IsActionJustPressed("ShootGrapplingHook");
            if (hookTrigger)
            {
                //_playerController2D.GrapplingHook.ShootHook(_playerController2D);
                _playerController2D.ChangeState(new Hooking(_playerController2D));
                return true;
            }
            return false;
        }
        #endregion

        private enum ApexModifierState
        {
            CanBeApplied,
            BeingApplied,
            WasApplied,
        }
    }
}
