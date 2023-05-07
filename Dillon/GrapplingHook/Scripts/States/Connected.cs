using Godot;
using PlayerController;
using PlayerController.States;

namespace GrapplingHook.States
{
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        public Connected(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter() 
        {
            // Attatch the player to the chain start node


            _grapplingHook.Physics.Initialize(_grapplingHook.GetHookStartPosition(),
                                              _grapplingHook.GetHookTargetPosition());
        }

        public override void Execute()
        {
            CheckTransitionToReturning();           

            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");

            _grapplingHook.Physics.ChainStart.ApplyImpulse(new Vector2(25, 0) * moveDirection);

            
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
                //_grapplingHook.PlayerController.Velocity = _grapplingHookJB.hookStart.LinearVelocity * 2;
                //_grapplingHook.PlayerController.ChangeState(new Jumping(_grapplingHook.PlayerController, false, _grapplingHookJB.hookStart.LinearVelocity * 1.5f));
            }
        }
    }
}
