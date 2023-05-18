using Godot;
using PlayerController;
using PlayerController.States;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook gets connected to the player.
    /// Grappling hook physics are initialized, 
    /// the players state changes to hooking,
    /// the players parent is set to the grappling hooks start node and
    /// the controls for the grappling hook get activated
    /// When the hotkey for the release of the grappling hokk is pressed the hook changes to the state returning.
    /// </summary>
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        public Connected(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter() 
        {           
            _grapplingHook.Physics.Initialize(_grapplingHook.GetHookStartPosition(),
                                              _grapplingHook.GetHookTargetPosition());

            _grapplingHook.SetPlayerAsChild();

            _grapplingHook.Physics.HookStart.SetControlsActive(true);
            _grapplingHook.Physics.HookStart.SetStartVelocity(_grapplingHook.GetPlayerVelocityOnStart());

            // Before changing the players state to hooking -> Player velocity is set to zero in the hooking state

            _grapplingHook.ChangePlayerStateToHooking();
        }

        public override void ExecuteProcess()
        {
            CheckTransitionToReturning();            
        }

        public override void ExecutePhysicsProcess() { }

        public override void Exit() 
        {
            // Detach the player from the hook start node
            _grapplingHook.RemovePlayerAsChild();

            _grapplingHook.Physics.HookStart.SetControlsActive(false);
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
