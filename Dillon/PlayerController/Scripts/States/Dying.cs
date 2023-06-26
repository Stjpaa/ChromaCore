using Godot;
using PlayerController;
using System;

namespace PlayerController.States
{
    public partial class Dying : State
    {
        public override string Name { get { return "Dying"; } }

        private uint _collisionMask;

        public Dying(PlayerController2D controller) : base(controller) { }

        public override void Enter()
        {
            _collisionMask = _playerController2D.CollisionMask;
            _playerController2D.CollisionMask = 0;
            _playerController2D.Velocity = Vector2.Zero;
            TransitionToFalling();
            _playerController2D.AnimatedSprite2D.Play("Death");
            _playerController2D.PlaySound("death_05");
        }
      
        public override void Exit() 
        {
            _playerController2D.CollisionMask = _collisionMask;
        }

        private void TransitionToFalling()
        {
            var callable = new Callable(_playerController2D, "RespawnAtLastCheckpoint");
            _playerController2D.AnimatedSprite2D.Connect(AnimatedSprite2D.SignalName.AnimationFinished, callable);
            
        }
    }
}
