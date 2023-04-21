using Godot;
using System;

public class Dashing : State
{
    public override string Name { get { return "Dash"; } }

    public static uint _availableDashes = 1;

    public Dashing(PlayerController2D playerController) : base(playerController) { }
    public override void Enter()
    {
        if (CheckFallingOrMovingTransition()) { return; }

        Dash();

        WaitForTimer();
    }
    public override void Execute(double delta) { }

    public override void Exit()
    {
        _playerController2D.AnimatedSprite2D.FlipH = false;

        _playerController2D.DashCooldownTimer.Start();
        _playerController2D.DashCooldownTimer.Timeout += OnDashTimerTimeout;
    }
    private void Dash()
    {
        var direction = Input.GetAxis("Move_Left", "Move_Right");
        var velocity = _playerController2D.Velocity;
        velocity.X = _playerController2D.DashSpeed * direction;
        _playerController2D.Velocity = velocity;
        _availableDashes -= 1;

        #region Animation
        //Left
        if (direction < 0)
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

    /// <summary>
    /// Waits for the timer and changes then back to the previous state - falling or moving
    /// </summary>
    private async void WaitForTimer()
    {
        await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(_playerController2D.DashDuration), SceneTreeTimer.SignalName.Timeout);
        _playerController2D.ChangeState(_playerController2D.previousState);
    }

    private void OnDashTimerTimeout()
    {
        _availableDashes++;
        _playerController2D.DashCooldownTimer.Timeout -= OnDashTimerTimeout;
    }

    #region Transitions
    private bool CheckFallingOrMovingTransition()
    {
        var direction = Input.GetAxis("Move_Left", "Move_Right");
        // To avoid Dash without direction
        if (direction == 0 || _availableDashes == 0)
        {
            _playerController2D.ChangeState(_playerController2D.previousState);
            return true;
        }
        return false;
    }
    #endregion
}
