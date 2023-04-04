using Godot;
using System;

public class Falling : State
{
    public override string Name { get { return "Falling"; } }

    public static float MAX_FALL_SPEED { get { return 700f; } }
    public static float FALL_SPEED_ACCELERATION { get { return 25f; } }
    public static float MAX_FALLING_MOVE_SPEED { get { return Moving.MAX_MOVE_SPEED * 0.9f; } }
    public static float FALLING_MOVE_SPEED_ACCELERATION { get { return Moving.MOVE_SPEED_ACCELERATION * 0.9f; } }

    private float _apexGravityModifier;
    private float _apexMovementModifier;
    private float _apex;
    private float _apexModifierDuration = 0.05f;

    private bool _jumpBuffering = false;

    private ApexModifierState _state;

    public Falling(PlayerController_2D playerController_2D) : base(playerController_2D) { }

    public override void Enter()
    {
        _playerController2D.AnimatedSprite2D.Play("InAir");
        _apexGravityModifier = 1f;
        _apexMovementModifier = 1f;
        _state = ApexModifierState.CanBeApplied;
    }
    public override void Execute(double delta)
    {
        ApplyMovement();

        if (_state == ApexModifierState.CanBeApplied)
        {
            UpdateApexModifier();
        }

        ApplyGravity();

        UpdateJumpBuffering();

        if (CheckTransitionToDash()) { return; }
        if (CheckTransitionToJump()) { return; }
        if (CheckTransitionToIdle()) { return; }
    }
    public override void Exit()
    {
        _playerController2D.Velocity = Vector2.Zero;
    }

    private void ApplyMovement()
    {
        var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
        var velocity = _playerController2D.Velocity;

        velocity.X += FALLING_MOVE_SPEED_ACCELERATION * _apexMovementModifier * moveDirection;

        if (_state is not ApexModifierState.BeingApplied)
        {
            velocity.X = Mathf.Clamp(velocity.X, -MAX_FALLING_MOVE_SPEED, MAX_FALLING_MOVE_SPEED);
        }

        _playerController2D.Velocity = velocity;
    }
    private void ApplyGravity()
    {
        var jumpEndTrigger = Input.IsActionJustReleased("Jump");
        var velocity = _playerController2D.Velocity;

        velocity.Y += (jumpEndTrigger) ? (FALL_SPEED_ACCELERATION * Jumping.JUMP_END_MODIFIER) : FALL_SPEED_ACCELERATION;
        velocity.Y = Mathf.Clamp(velocity.Y, float.MinValue, MAX_FALL_SPEED);

        velocity.Y *= _apexGravityModifier;
        _playerController2D.Velocity = velocity;
    }

    private void UpdateApexModifier()
    {
        var velocity = _playerController2D.Velocity;
        if (_apex > velocity.Y && _playerController2D.previousState is Jumping)
        {
            _apex = velocity.Y;
        }
        else if (_apex <= velocity.Y && _apex < 0 && _state is ApexModifierState.CanBeApplied)
        {
            if (velocity.Y >= 0)
            {
                // Apex modifier will be applied when the player reaches the highes position during the jump
                // This is when his velocity is >= 0
                ApplyApexModifier();
            }
        }
    }
    private async void ApplyApexModifier()
    {
        _state = ApexModifierState.BeingApplied;
        _apexGravityModifier = 0f;
        _apexMovementModifier = 2f;

        await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(_apexModifierDuration), SceneTreeTimer.SignalName.Timeout);

        _state = ApexModifierState.WasApplied;
        _apexGravityModifier = 1f;
        _apexMovementModifier = 1f;
    }

    private void UpdateJumpBuffering()
    {
        if (_jumpBuffering == false)
        {
            _jumpBuffering = Input.IsActionJustPressed("Jump");
        }
    }

    private bool CheckTransitionToIdle()
    {
        if (_playerController2D.IsOnFloor())
        {
            _playerController2D.ChangeState(new Idle(_playerController2D));
            return true;
        }
        return false;
    }
    private bool CheckTransitionToDash()
    {
        var dashTrigger = Input.IsActionJustPressed("Dash");
        var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
        if (dashTrigger && moveDirection != 0 && Dashing._availableDashes != 0)
        {
            _playerController2D.ChangeState(new Dashing(_playerController2D));
            return true;
        }
        return false;
    }
    private bool CheckTransitionToJump()
    {
        if (_jumpBuffering && _playerController2D.IsOnFloor())
        {
            _playerController2D.ChangeState(new Jumping(_playerController2D));
            return true;
        }
        return false;
    }

    private enum ApexModifierState
    {
        CanBeApplied,
        BeingApplied,
        WasApplied,
    }
}
