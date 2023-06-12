using Godot;
using System;

namespace PlayerController.States
{
    /// <summary>
    /// This state is used when the player is moving.
    /// 
    /// <para> When a movement key is pressed acceleration is applied otherwise decceleration is applied. </para>
    /// 
    /// <para> Transition to jumping --> The player triggers the jump key. </para>
    /// <para> Transition to dashing --> The player triggers the dash key and the dash is avialable. </para>
    /// <para> Transition to falling --> The player triggers the grappling hook key. </para>
    /// <para> Transition to hooking --> The player is in air. </para>
    /// <para> Transition to idle --> The player stops moving. </para>
    /// </summary>
    public class Moving : State
    {
        public override string Name { get { return "Move"; } }

        private bool _movingLeft;
        private float _moveDirection;

        private string _walkingAnimation = "Walk";
        private string _pushingAnimation = "Push";

        private double _step_timer = 0.0;

        public Moving(PlayerController2D controller) : base(controller) { }
        public override void Enter()
        {
            _movingLeft = Input.IsActionPressed("Move_Left");          
        }
        public override void ExecutePhysicsProcess(double delta)
        {
            _moveDirection = Input.GetAxis("Move_Left", "Move_Right");

            _step_timer -= delta;
            if(_step_timer < 0.0)
            {
                _playerController2D.PlaySound("step");
                _step_timer = 0.3;
            }

            if (CheckTransitionToJumping()) { return; }
            if (CheckTransitionToDashing()) { return; }
            if (CheckTransitionToHooking()) { return; }           
            
            #region Movement         
            if (_moveDirection != 0 && _playerController2D.IsOnFloor())
            {
                Acceleration(_moveDirection);
            }
            else
            {
                Decerleration();
            }
            #endregion

            #region Animation
            // Only detect objects
            uint collisionMask = 1;
            collisionMask <<= 7;

            var spaceState = _playerController2D.GetWorld2D().DirectSpaceState;
            var leftQuery = PhysicsRayQueryParameters2D.Create(_playerController2D.GlobalPosition, 
                                                           _playerController2D.GlobalPosition + new Vector2(-15, 0),
                                                           collisionMask);
            var rightQuery = PhysicsRayQueryParameters2D.Create(_playerController2D.GlobalPosition,
                                                               _playerController2D.GlobalPosition + new Vector2(15, 0),
                                                               collisionMask);
            var leftCollisionCheck = spaceState.IntersectRay(leftQuery);
            var rightCollisionCheck = spaceState.IntersectRay(rightQuery);

            if (leftCollisionCheck.Count > 0 || rightCollisionCheck.Count > 0)
            {
                _playerController2D.AnimatedSprite2D.Play(_pushingAnimation);
            }
            else { _playerController2D.AnimatedSprite2D.Play(_walkingAnimation); }

            // Left
            if (_moveDirection < 0)
            {
                _playerController2D.AnimatedSprite2D.FlipH = true;
            }
            // Right
            else if (_moveDirection > 0)
            {
                _playerController2D.AnimatedSprite2D.FlipH = false;
            }

            #endregion

            if (CheckTransitionToFalling()) { return; }
            if (CheckTransitionToIdle()) { return; }
        }
        public override void Exit() { }

        /// <summary>
        /// Applies a acceleration on the playerController.
        /// </summary>
        private void Acceleration(float direction)
        {
            var velocity = _playerController2D.Velocity;
            velocity.X += _playerController2D.MoveSpeedAcceleration * direction;
            velocity.X = Mathf.Clamp(velocity.X, -_playerController2D.MaxMoveSpeed, _playerController2D.MaxMoveSpeed);
            _playerController2D.Velocity = velocity;
        }

        /// <summary>
        /// Applies a deceleration opposite to the playerControllers direction of movement.
        /// </summary>
        private void Decerleration()
        {
            var velocity = _playerController2D.Velocity;
            if (_movingLeft)
            {
                velocity.X += _playerController2D.MoveSpeedDecceleration;
                velocity.X = Mathf.Clamp(velocity.X, -_playerController2D.MaxMoveSpeed, 0);
            }
            else
            {
                velocity.X -= _playerController2D.MoveSpeedDecceleration;
                velocity.X = Mathf.Clamp(velocity.X, 0, _playerController2D.MaxMoveSpeed);
            }
            _playerController2D.Velocity = velocity;
        }

        #region Transitions
        protected bool CheckTransitionToJumping()
        {
            var jumpPressedTrigger = Input.IsActionJustPressed("Jump");
            if (jumpPressedTrigger)
            {
                _playerController2D.PlaySound("jump_01");
                _playerController2D.ChangeState(new Jumping(_playerController2D, true));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDashing()
        {
            var dashTrigger = Input.IsActionJustPressed("Dash");
            if (dashTrigger && _moveDirection != 0 && Dashing._availableDashes != 0)
            {
                _playerController2D.ChangeState(new Dashing(_playerController2D, _moveDirection));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToFalling()
        {
            if (_playerController2D.IsOnFloor() == false)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D, false, true));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToIdle()
        {
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            if (_playerController2D.Velocity.X == 0 &&
                        moveDirection == 0)
            {
                _playerController2D.ChangeState(new Idle(_playerController2D));
                return true;
            }
            return false;
        }

        private bool CheckTransitionToHooking()
        {
            var hookTrigger = Input.IsActionJustPressed("ShootGrapplingHook");
            if (hookTrigger)
            {
                return _playerController2D.ShootGrapplingHook();
            }
            return false;
        }
        #endregion
    }
}
