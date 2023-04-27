using Godot;
using System;

namespace PlayerController.States
{
    public class Moving : State
    {
        public override string Name { get { return "Move"; } }

        private bool _movingLeft;

        public Moving(PlayerController2D controller) : base(controller) { }
        public override void Enter()
        {
            _movingLeft = Input.IsActionPressed("Move_Left");
        }
        public override void Execute()
        {
            if (CheckTransitionToJumping()) { return; }
            if (CheckTransitionToDashing()) { return; }
            //if (CheckTransitionToHooking()) { return; }           
            
            #region Movement
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");

            if (moveDirection != 0 && _playerController2D.IsOnFloor())
            {
                Acceleration(moveDirection);
            }
            else
            {
                Decerleration();
            }
            #endregion

            #region Animation
            // Left
            if (moveDirection < 0)
            {
                _playerController2D.AnimatedSprite2D.Play("Walk");
                _playerController2D.AnimatedSprite2D.FlipH = true;
            }
            // Right
            else if (moveDirection > 0)
            {
                _playerController2D.AnimatedSprite2D.Play("Walk");
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
        /// Applies a deceleration opposite to the playerControllers direction of movement
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
                _playerController2D.ChangeState(new Jumping(_playerController2D, true));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDashing()
        {
            var dashTrigger = Input.IsActionJustPressed("Dash");
            if (dashTrigger && Dashing._availableDashes != 0)
            {
                _playerController2D.ChangeState(new Dashing(_playerController2D));
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
                //_playerController2D.GrapplingHook.ShootHook(_playerController2D);
                _playerController2D.ChangeState(new Hooking(_playerController2D));
                return true;
            }
            return false;
        }
        #endregion
    }
}
