using Godot;
using PlayerController;
using System;

namespace PlayerController.States
{
    public partial class Dying : State
    {
        public override string Name { get { return "Dying"; } }

        public Dying(PlayerController2D controller) : base(controller) { }

        public override void Enter()
        {
            _playerController2D.Velocity = Vector2.Zero;
            TransitionToFalling();
            _playerController2D.AnimatedSprite2D.Play("Death");
        }
      
        public override void Exit() { }

        private void TransitionToFalling()
        {
            var callable = new Callable(_playerController2D, "RespawnAtLastCheckpoint");
            _playerController2D.AnimatedSprite2D.Connect(AnimatedSprite2D.SignalName.AnimationFinished, callable);
            
        }
    }
}
