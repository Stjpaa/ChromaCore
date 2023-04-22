using Godot;
using PlayerController;

namespace GrapplingHook.States
{
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        private PlayerController2D _playerController;
        private HookableArea _hookableArea;

        public Connected(GrapplingHook grapplingHook) : base(grapplingHook) 
        {
            _playerController = grapplingHook.PlayerController;
        }

        public override void Enter() 
        {
            _playerController.Velocity = Vector2.Zero;
            _hookableArea = _grapplingHook.rayCast2Ds.Find(o => o.GetCollider() != null).GetCollider() as HookableArea;

            _hookableArea.GrapplingHookRotation.GlobalPosition = _grapplingHook.GlobalPosition;
            _hookableArea.PlayerControllerPosition.GlobalPosition = _playerController.HookStartPosition;
        }

        public override void Execute()
        {
            CheckTransitionToReturning();

            var direction = Input.GetAxis("Move_Right", "Move_Left");
            var speed = 1f;
            _hookableArea.GrapplingHookRotation.Rotation += speed * direction * (float)_grapplingHook.GetProcessDeltaTime();
            _playerController.GlobalPosition = _hookableArea.PlayerControllerPosition.GlobalPosition + new Vector2(0, 32);
        }

        public override void Exit() 
        { 

        }

        private void CheckTransitionToReturning()
        {
            var returnTrigger = Input.IsActionJustPressed("ReleaseGrapplingHook");
            if (returnTrigger)
            {
                _grapplingHook.ChangeState(new Returning(_grapplingHook));
            }
        }
    }
}
