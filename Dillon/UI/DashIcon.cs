using Godot;
using System;

namespace UI.DashIcon
{
    /// <summary>
    /// Simple progress animation with a blinking effect at the end.
    /// </summary>
    public partial class DashIcon : TextureProgressBar
    {
        private AnimationPlayer _player;
        private Tween _tween;

        public override void _Ready()
        {
            _player = GetNode<AnimationPlayer>("AnimationPlayer");
            PlayerController.States.Dashing.OnDashEnd += StartCooldownAnimation;
        }

        public override void _ExitTree()
        {
            PlayerController.States.Dashing.OnDashEnd -= StartCooldownAnimation;
        }

        public void StartCooldownAnimation(float timerDuration)
        {
            // Progress bar animation
            Value = MaxValue;
            _tween = GetTree().CreateTween();
            _tween.TweenProperty(this, "value", 0, timerDuration);
            _tween.Finished += EndCooldownAnimation;
        }

        private void EndCooldownAnimation()
        {
            // Start the blinking animation
            _player.Play("DashReady");
            _tween.Kill();
        }
    }
}
