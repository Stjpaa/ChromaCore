using Godot;
using PlayerController;
using PlayerController.States;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook gets connected to the player.
    /// <para>
    /// Grappling hook physics are initialized, 
    /// the players state changes to hooking,
    /// the players parent is set to the grappling hooks start node and
    /// the controls for the grappling hook get activated.
    /// </para>
    /// When the hotkey for the release of the grappling hook is pressed the hook changes to the state returning.
    /// </summary>
    public class Connected : State
    {
        public override string Name { get { return "Connected"; } }

        public Connected(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter() 
        {
            _grapplingHook.Physics.Initialize(_grapplingHook.GetHookStartPosition(),
                                              _grapplingHook.GetHookTargetPosition());
                                         
            _grapplingHook.Physics.HookStart.SetControlsActive(true);
            _grapplingHook.Physics.HookStart.SetStartVelocity(_grapplingHook.GetPlayerVelocityOnStart());

            SetStartImpulse();
            SetPlayerPosition();
        }
      
        public override void ExecuteProcess()
        {
            _grapplingHook.UpdateChainEndPosition();
            CheckTransitionToReturning();
        }

        public override void Exit() 
        {
            _grapplingHook.RemoteTransform.RemotePath = null;

            _grapplingHook.Physics.HookStart.SetControlsActive(false);
        }

        private void SetStartImpulse()
        {
            // Apply a impulse if the player starts the hook in the given angle 
            // -> To avoid that the player moves slow at the start
            var direction = _grapplingHook.GetHookStartPosition() - _grapplingHook.GetHookTargetPosition();
            var angle = Mathf.RadToDeg(direction.Angle());

            var startImpulse = Vector2.Zero;

            // Left
            if (angle <= -90 && angle >= -110)
            {
                startImpulse.X = -_grapplingHook.StartImpulse;
            }// Right
            else if (angle > -90 && angle <= -70)
            {
                startImpulse.X = _grapplingHook.StartImpulse;
            }

            _grapplingHook.Physics.HookStart.SetStartImpulse(startImpulse);
        }

        private async void SetPlayerPosition()
        {
            await _grapplingHook.ToSignal(_grapplingHook.GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
            _grapplingHook.RemoteTransform.RemotePath = _grapplingHook.GetPlayerNodePath();
            _grapplingHook.ChangePlayerStateToHooking();
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
