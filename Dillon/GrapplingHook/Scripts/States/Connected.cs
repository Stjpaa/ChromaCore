using Godot;
using PlayerController;
using PlayerController.States;

namespace GrapplingHook.States
{
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        private PlayerController2D _playerController;
        private HookableArea _hookableArea;
        private Rope _rope;
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

            _rope = _grapplingHook.rope.Instantiate() as Rope;
            _grapplingHook.GetParent().AddChild(_rope);

            _rope.SpawnRope(_grapplingHook.HookStartPosition, _grapplingHook._globalTargetPosition);          
        }

        public override void Execute()
        {
            CheckTransitionToReturning();

            var newPos = _rope.GetNode<RigidBody2D>("RopeStartPiece").GlobalPosition;
            newPos.Y += 32;
            _playerController.GlobalPosition = newPos;
            var direction = Input.GetAxis("Move_Left", "Move_Right");
            _rope.GetNode<RigidBody2D>("RopeStartPiece").ApplyImpulse(Vector2.Right * direction * 20f);
            GD.Print(direction);
        }

        public override void Exit() 
        {
            _rope.QueueFree();
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
