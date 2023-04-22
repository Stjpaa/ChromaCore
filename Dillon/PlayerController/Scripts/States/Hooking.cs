

using Godot;
using GrapplingHook.States;

namespace PlayerController.States
{
    public partial class Hooking : State
    {
        public override string Name { get { return "Hooking"; } }

        public Hooking(PlayerController2D playerController2D) : base(playerController2D) { }


        public override void Enter()
        {
            _playerController2D.GrapplingHook.ShootHook(_playerController2D);
        }

        public override void Execute()
        {
            CheckTransitionToPreviousState();

            if (_playerController2D.GrapplingHook._state is Connected == false)
            {
                //Gravity
                var velocity = _playerController2D.Velocity;
                velocity.Y += _playerController2D.FallSpeedAcceleration;
                velocity.Y = Mathf.Clamp(velocity.Y, float.MinValue, _playerController2D.MaxFallSpeed);
                _playerController2D.Velocity = velocity;
            }         
        }

        public override void Exit()
        {

        }

        private bool CheckTransitionToPreviousState()
        {
            var releaseTrigger = Input.IsActionJustPressed("ReleaseGrapplingHook");
            if(releaseTrigger)
            {
                _playerController2D.ChangeState(_playerController2D.previousState);
            }
            return releaseTrigger;
        }
    }
}
