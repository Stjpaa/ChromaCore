using Godot;
using System;

namespace GrapplingHook.States
{
    /// <summary>
    /// In this state the grappling hook moves back to the start position (player).
    /// When the start position is reached the grappling hook changes his state to idle.
    /// </summary>
    public class Returning : State
    {
        public override string Name { get { return "Returning"; } }

        public Returning(GrapplingHook grapplingHook) : base(grapplingHook) { }

        private Tween _tween;

        public override void Enter() 
        {
            // Returning animation of the grappling hook
            _tween = _grapplingHook.GetTree().CreateTween();
            _tween.TweenProperty(_grapplingHook.Hook, "position", _grapplingHook.GetHookStartPosition(), 0.1f);
            _tween.Finished += TransitionToIdle;
        }

        public override void ExecuteProcess() 
        {
            _grapplingHook.UpdateChainEndPosition();
        }

        public override void Exit() { }

        private void TransitionToIdle()
        {
            _tween.Finished -= TransitionToIdle;
            _grapplingHook.ChangeState(new Idle(_grapplingHook));
        }
    }
}
