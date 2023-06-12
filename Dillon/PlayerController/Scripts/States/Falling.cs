using Godot;
using System;

namespace PlayerController.States
{
    /// <summary>
    /// This state is used when the player is in air.
    /// The player is able to move and gravity is constantly applied.
    /// 
    /// <para>
    /// Implemented platformer mechanics:
    /// - Apex modifier
    /// - Variable jump height
    /// - Coyote time
    /// </para>
    /// 
    /// <para> Mechanics and custom gravity can be set in the constructor. </para>
    /// 
    /// <para> Transition to idle --> The player is on the ground, is not moving and no custom gravity is applied. </para>
    /// <para> Transition to moving --> The player is on the ground, is not moving and no custom gravity is applied. </para>
    /// <para> Transition to dashing --> The player triggers the dash key, is moving and the dash is available. </para>
    /// <para> Transition to jumping --> The player pressed the jump key, is on the ground and no custom gravity is applied. </para>
    /// <para> Transition to hooking --> The player pressed the grappling hook key and no custom gravity is applied. </para>
    /// </summary>
    public class Falling : State
    {
        public override string Name { get { return "Falling"; } }

        private float _apexGravityModifier;
        private float _apexMovementModifier;
        private float _apex;
        private float _timer;
        private float _moveDirection;

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
            _playerController2D.AnimatedSprite2D.Play("Falling");
            _apexGravityModifier = 1f;
            _apexMovementModifier = 1f;
            _state = ApexModifierState.CanBeApplied;
        }

        public override void ExecutePhysicsProcess()
        {
            if (ApplyCoyoteTime((float)_playerController2D.GetPhysicsProcessDeltaTime())) { return; }

            _moveDirection = Input.GetAxis("Move_Left", "Move_Right");

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
            if (CheckTransitionToMove()) { return; }
            if (CheckTransitionToIdle()) { return; }
        }
        public override void Exit()
        {
            if (_playerController2D.IsOnFloor() && Input.GetAxis("Move_Left", "Move_Right") == 0)
            {
                _playerController2D.Velocity = Vector2.Zero;
            }

            if (_playerController2D.PreviousState is not Hooking)
            {
                _playerController2D.FollowingCamera.Mode = FollowingCamera.FollowingCamera.CameraModes.Normal;
            }
        }

        private void ApplyMovement()
        {          
            var velocity = _playerController2D.Velocity;

            if (_moveDirection != 0)
            {
                velocity.X += _playerController2D.FallingMoveSpeedAcceleration * _apexMovementModifier * _moveDirection; 
                if(_moveDirection < 0) { _playerController2D.AnimatedSprite2D.FlipH = true; }
                else { _playerController2D.AnimatedSprite2D.FlipH = false; }
            }
            else
            {
                if (velocity.X > 0)
                {
                    velocity.X -= _playerController2D.FallingMoveSpeedDeceleration;
                }
                else if (velocity.X < 0)
                {
                    velocity.X += _playerController2D.FallingMoveSpeedDeceleration;
                }
            }

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

            if (_timer > _playerController2D.CoyoteTimeDuration) { return false; }

            if (Input.IsActionJustPressed("Jump") && _applyCoyoteTime)
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
            if (_apex > velocity.Y && _playerController2D.PreviousState is Jumping)
            {
                _apex = velocity.Y;
            }
            else if (_apex <= velocity.Y && _apex < 0 && _state is ApexModifierState.CanBeApplied)
            {
                if (velocity.Y >= 0)
                {
                    // Apex modifier will be applied when the player reaches the highes position during the jump
                    // This is when the velocity is >= 0
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

        private bool CheckTransitionToMove()
        {
            if (_playerController2D.IsOnFloor() && _moveDirection != 0 && _applyCustomGravity == false)
            {
                _playerController2D.TriggerLandingParticles();
                _playerController2D.ChangeState(new Moving(_playerController2D));
                return true;
            }
            return false;
        }

        private bool CheckTransitionToIdle()
        {
            if (_playerController2D.IsOnFloor() && _moveDirection == 0 && _applyCustomGravity == false)
            {
                _playerController2D.TriggerLandingParticles();
                _playerController2D.ChangeState(new Idle(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDash()
        {
            var dashTrigger = Input.IsActionJustPressed("Dash");
            if (dashTrigger && _moveDirection != 0 && Dashing._availableDashes > 0)
            {
                _playerController2D.ChangeState(new Dashing(_playerController2D, _moveDirection));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToJumping()
        {
            if (_jumpBuffering && _playerController2D.IsOnFloor() && _applyCustomGravity == false)
            {
                _playerController2D.TriggerLandingParticles();
                _playerController2D.ChangeState(new Jumping(_playerController2D, true));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToHooking()
        {
            var hookTrigger = Input.IsActionJustPressed("ShootGrapplingHook");
            if (hookTrigger && _applyCustomGravity == false)
            {
                return _playerController2D.ShootGrapplingHook();
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
