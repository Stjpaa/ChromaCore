using Godot;
using PlayerController;
using PlayerController.States;

namespace GrapplingHook.States
{
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        private PlayerController2D _playerController;
        private GrapplingHookJointBased _grapplingHookJB;
        private HookableArea _hookableArea;
        private Vector2 _playerVelocity;

        public Connected(GrapplingHook grapplingHook) : base(grapplingHook) 
        {
            _playerController = grapplingHook.PlayerController;
        }

        public override void Enter() 
        {
            _playerVelocity = _playerController.Velocity;
            _playerController.Velocity = Vector2.Zero;
            _hookableArea = _grapplingHook.rayCast2Ds.Find(o => o.GetCollider() != null).GetCollider() as HookableArea;

            _grapplingHookJB = _grapplingHook.grapplingHookJB.Instantiate<GrapplingHookJointBased>();
            _grapplingHook.GetParent().AddChild(_grapplingHookJB);
            _grapplingHookJB.InitializeHook(_grapplingHook.GlobalPosition, _playerController.HookStartPosition + new Vector2(0, 32), Mathf.RoundToInt(_grapplingHook.GetCurrentHookLenght()));
        }

        public override void Execute()
        {
            CheckTransitionToReturning();

            _playerController.GlobalPosition = _grapplingHookJB.hookEnd.GlobalPosition;

            if(Input.IsActionJustPressed("Move_Left"))
            {
                _grapplingHookJB.hookEnd.ApplyImpulse(new Vector2(-300, 0));
            }
            if (Input.IsActionJustPressed("Move_Right"))
            {
                _grapplingHookJB.hookEnd.ApplyImpulse(new Vector2(300, 0));
            }
        }

        public override void Exit() 
        {
            _grapplingHookJB.QueueFree();
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
