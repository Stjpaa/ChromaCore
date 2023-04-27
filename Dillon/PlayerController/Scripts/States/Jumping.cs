using Godot;
using System;

namespace PlayerController.States
{
    public class Jumping : State
    {
        public override string Name { get { return "Jumping"; } }

        private Vector2 _jumpVelocity;

        private bool _applyVariableJumpHeight;

        public Jumping(PlayerController2D playerController2D, bool applyVariableJumpHeight, Vector2 jumpVelocity) : base(playerController2D)
        {
            _jumpVelocity = jumpVelocity;
            _applyVariableJumpHeight = applyVariableJumpHeight;
        }
        public Jumping(PlayerController2D playerContoller2D, bool applyVariableJumpHeight) : base(playerContoller2D)
        {
            _jumpVelocity = new Vector2(0, -_playerController2D.JumpForce);
            _applyVariableJumpHeight = applyVariableJumpHeight;
        }

        public override void Enter()
        {
            Jump();
        }

        public override void Execute()
        {
            if (CheckFallingTransition()) { return; }
        }
        public override void Exit() { }

        private void Jump()
        {
            _playerController2D.Velocity = _jumpVelocity;
        }

        #region Transitions
        private bool CheckFallingTransition()
        {
            if (_playerController2D.IsOnFloor() == false)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D, _applyVariableJumpHeight, false));
                return true;
            }
            return false;
        }
        #endregion
    }
}
