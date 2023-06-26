using Godot;
using System;

namespace PlayerController.States
{
    /// <summary>
    /// This state is used when the player dashes. 
    /// <para> The dash velocity is continuously applied during the dash. </para>
    /// <para> Transition to moving --> When the timer ends and the player is on the ground. </para>
    /// <para> Transition to falling --> When the timer ends and the player is in air. </para>
    /// </summary>
    public class Dashing : State
    {
        public override string Name { get { return "Dash"; } }

        public static uint _availableDashes = 1;

        public static event Action<float> OnDashEnd;

        private float _direction;
        private float _timer = 0;

        public Dashing(PlayerController2D playerController, float direction) : base(playerController) 
        {
            _direction = direction;
        }
        public override void Enter()
        {
            _availableDashes--;
            _playerController2D.PlaySound("dash");
        }

        public override void ExecutePhysicsProcess(double delta)
        {
            _timer += (float)_playerController2D.GetPhysicsProcessDeltaTime();
            if( _timer > _playerController2D.DashDuration)
            {
                if(TransistionToMoving()) { return; }
                if(TransitionToFalling()) { return; }
            }
            else
            {
                Dash();
            }         
        }

        public override void Exit()
        {
            _playerController2D.AnimatedSprite2D.FlipH = false;

            _playerController2D.DashCooldownTimer.Start();
            OnDashEnd?.Invoke(_playerController2D.DashCooldown);
            _playerController2D.DashCooldownTimer.Timeout += OnDashTimerTimeout;
        }
        private void Dash()
        {
            var velocity = _playerController2D.Velocity;
            velocity.X = _playerController2D.DashSpeed * _direction;
            velocity.Y = 0;
            _playerController2D.Velocity = velocity;

            #region Animation
            //Left
            if (_direction < 0)
            {
                _playerController2D.AnimatedSprite2D.FlipH = true;
            }
            //Right
            else
            {
                _playerController2D.AnimatedSprite2D.FlipH = false;
            }
            _playerController2D.AnimatedSprite2D.Play("Dash");
            #endregion
        }

        private void OnDashTimerTimeout()
        {
            _availableDashes++;
            _playerController2D.DashCooldownTimer.Timeout -= OnDashTimerTimeout;
        }

        #region Transitions
        private bool TransistionToMoving()
        {
            if(_playerController2D.IsOnFloor())
            {
                _playerController2D.PlaySound("landing");
                _playerController2D.TriggerLandingParticles();
                _playerController2D.ChangeState(new Moving(_playerController2D));
                return true;
            }
            return false;
        }

        private bool TransitionToFalling()
        {
            if(_playerController2D.IsOnFloor() == false)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D));
                return true;
            }
            return false;
        }
        #endregion
    }
}
