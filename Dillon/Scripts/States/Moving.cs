using Godot;
using System;

public class Moving : State
{
    public override string Name { get { return "Move"; } }

    public static float MAX_MOVE_SPEED { get { return 250f; } }
    public static float MOVE_SPEED_ACCELERATION { get { return 40f; } }
    public static float MOVE_SPEED_DECCELERATION { get { return 40f; } }

    private bool _movingLeft;

    private float _coyoteTime = 0.08f;
    private bool _coyoteTimeTriggered;

    public Moving(PlayerController_2D controller) : base(controller) { }
    public override void Enter()
    {
        _movingLeft = Input.IsActionPressed("Move_Left");
    }
    public override void Execute(double delta)
    {
        if (CheckTransitionToJumping()) { return; }
        if (CheckTransitionToDashing()) { return; }       

        #region Movement
        var moveDirection = Input.GetAxis("Move_Left", "Move_Right");

        if (moveDirection != 0 && _playerController2D.IsOnFloor())
        {
            Acceleration(moveDirection);
        }
        else
        {
            Decerleration();
        }
        #endregion

        #region Animation
        // Left
        if (moveDirection < 0)
        {
            _playerController2D.AnimatedSprite2D.Play("Walk");
            _playerController2D.AnimatedSprite2D.FlipH = true;
        }
        // Right
        else if (moveDirection > 0)
        {
            _playerController2D.AnimatedSprite2D.Play("Walk");
            _playerController2D.AnimatedSprite2D.FlipH = false;
        }

        #endregion

        if (_coyoteTimeTriggered) { return; }

        if (CheckTransitionToFalling()) { return; }
        if (CheckTransitionToIdle()) { return; }
    }
    public override void Exit() { }

    /// <summary>
    /// Applies a acceleration on the playerController.
    /// </summary>
    private void Acceleration(float direction)
    {
        var velocity = _playerController2D.Velocity;
        velocity.X += MOVE_SPEED_ACCELERATION * direction;
        velocity.X = Mathf.Clamp(velocity.X, -MAX_MOVE_SPEED, MAX_MOVE_SPEED);
        _playerController2D.Velocity = velocity;
    }

    /// <summary>
    /// Applies a deceleration opposite to the playerControllers direction of movement
    /// </summary>
    private void Decerleration()
    {
        var velocity = _playerController2D.Velocity;
        if (_movingLeft)
        {
            velocity.X += MOVE_SPEED_DECCELERATION;
            velocity.X = Mathf.Clamp(velocity.X, -MAX_MOVE_SPEED, 0);
        }
        else
        {
            velocity.X -= MOVE_SPEED_DECCELERATION;
            velocity.X = Mathf.Clamp(velocity.X, 0, MAX_MOVE_SPEED);
        }
        _playerController2D.Velocity = velocity;
    }

    private async void ApplyCoyoteTime()
    {
        _coyoteTimeTriggered = true;
        await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(_coyoteTime), SceneTreeTimer.SignalName.Timeout);
        _playerController2D.ChangeState(new Falling(_playerController2D));
    }

    private bool CheckTransitionToJumping()
    {
        var jumpTrigger = Input.IsActionJustPressed("Jump");
        if (jumpTrigger)
        {
            _playerController2D.ChangeState(new Jumping(_playerController2D));
            return true;
        }
        return false;
    }
    private bool CheckTransitionToDashing()
    {
        var dashTrigger = Input.IsActionJustPressed("Dash");
        if (dashTrigger && Dashing._availableDashes != 0)
        {
            _playerController2D.ChangeState(new Dashing(_playerController2D));
            return true;
        }
        return false;
    }
    private bool CheckTransitionToFalling()
    {
        if (_playerController2D.Velocity.X == 0 &&
                _playerController2D.IsOnFloor() == false)
        {
            ApplyCoyoteTime();
            return true;
        }
        return false;
    }
    private bool CheckTransitionToIdle()
    {
        var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
        if (_playerController2D.Velocity.X == 0 &&
                _playerController2D.IsOnFloor() &&
                    moveDirection == 0)
        {
            _playerController2D.ChangeState(new Idle(_playerController2D));
            return true;
        }
        return false;
    }
}
