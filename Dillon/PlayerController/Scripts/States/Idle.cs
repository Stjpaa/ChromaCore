using Godot;

namespace PlayerController.States
{
    /// <summary>
    /// This state is used when the player is on the ground and is not moving.
    /// 
    /// <para> Transition to jumping --> The player triggers the jump key </para> 
    /// <para> Transition to moving --> The player triggers the moving keys. </para>
    /// </summary>
    public class Idle : State
    {
        public override string Name { get { return "Idle"; } }

        public Idle(PlayerController2D controller) : base(controller) { }
        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("Idle");
        }
        public override void ExecutePhysicsProcess()
        {
            if (CheckTransitionToJumping()) { return; }
            if (CheckTransitionToMoving()) { return; }
        }
        public override void Exit() { }

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
        private bool CheckTransitionToMoving()
        {
            var axis = Input.GetAxis("Move_Left", "Move_Right");
            if (axis != 0)
            {
                _playerController2D.ChangeState(new Moving(_playerController2D));
                return true;
            }
            return false;
        }
        #endregion
    }
}