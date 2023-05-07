using Godot;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook is not visible and listens to the shoot event.
    /// If the shoot events gets invoked the hook is set visible and changes his state to shooting.
    /// Process and PhysicProcess of the grappling hook are deactivated during this state.
    /// </summary>
    public class Idle : State
    {
        public override string Name { get { return "Idle"; } }

        public Idle(GrapplingHook grapplingHook) : base(grapplingHook) { }

        public override void Enter()
        {
            SetGrapplingHookPassive();
        }

        public override void Execute() { }

        public override void Exit()
        {
            SetGrapplingHookActive();
        }

        private void SetGrapplingHookActive()
        {
            _grapplingHook.Hook.Visible = true;
            _grapplingHook.Chain.Visible = true;

            _grapplingHook.SetProcess(true);
            _grapplingHook.SetPhysicsProcess(true);

            _grapplingHook.ShootEvent -= TransitionToShooting;
        }
        private void SetGrapplingHookPassive()
        {
            _grapplingHook.Hook.Visible = false;
            _grapplingHook.Chain.Visible = false;

            _grapplingHook.SetProcess(false);
            _grapplingHook.SetPhysicsProcess(false);

            _grapplingHook.ShootEvent += TransitionToShooting;
        }

        private void TransitionToShooting()
        {
            _grapplingHook.ChangeState(new Shooting(_grapplingHook));
        }
    }
}
