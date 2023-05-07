

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
            _playerController2D.AnimatedSprite2D.Play("Hooking");
            //_playerController2D.Velocity = Vector2.Zero;
        }

        public override void Execute()
        {
            CheckTransitionToPreviousState();

            var velocity = _playerController2D.Velocity;



                velocity.Y += _playerController2D.FallSpeedAcceleration;


            velocity.Y = Mathf.Clamp(velocity.Y, float.MinValue, _playerController2D.MaxFallSpeed);


            _playerController2D.Velocity = velocity;
        }

        public override void Exit()
        {

        }

        private bool CheckTransitionToPreviousState()
        {
            var releaseTrigger = Input.IsActionJustPressed("ReleaseGrapplingHook");
            if(releaseTrigger)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D));
            }
            return releaseTrigger;
        }
    }
}
