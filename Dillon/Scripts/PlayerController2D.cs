using Godot;
using System;

public partial class PlayerController2D : CharacterBody2D
{
    // GitHub Commit logs Version 0.0.10
    // Added variable for apex modifier movement boost to the data and the playercontroller
    // Falling State:
    // - Variable jump height can now be turned on or off by a parameter
    // - Coyote time can now be turned on or off by a parameter
    // - Custom gravity can be applied but without mechanics
    //
    // - Implemented interaction with jumppads
    // - Implemented interaction with gravity fiels
    // - Implemented interaction with checkpoints
    // - Implemented interaction with portals
    // - Added grappling hook visuals
    // Problems:
    // - Coyote time can be used everytime the Falling state is entered -> solved
    // - Dash works not correctly inside a gravity field -> open

    [Export]
    public PlayerController2D_Data data;

    public AnimatedSprite2D AnimatedSprite2D
    {
        get;
        private set;
    }

    public Timer DashCooldownTimer
    {
        get;
        private set;
    }


    public Transform2D CheckpointPosition
    {
        get { return _checkpointPosition; }
        private set { _checkpointPosition = value; }
    }


    #region Moving State Variables
    public float MaxMoveSpeed { get { return data.maxMoveSpeed; } }
    public float MoveSpeedAcceleration { get { return data.moveSpeedAcceleration; } }
    public float MoveSpeedDecceleration { get { return data.moveSpeedDecceleration; } }
    #endregion
    #region Falling State Variables
    public float MaxFallSpeed { get { return data.maxFallSpeed; } }
    public float FallSpeedAcceleration { get { return data.fallSpeedAcceleration; } }
    public float MaxFallingMoveSpeed { get { return data.maxFallingMoveSpeed; } }
    public float FallingMoveSpeedAcceleration { get { return data.fallingMoveSpeedAcceleration; } }
    #endregion
    #region Jumping State Variables
    public float JumpForce { get { return data.jumpForce; } }
    public float JumpEndModifier { get { return data.jumpEndModifier; } }
    #endregion
    #region Dashing State Variables
    public float DashSpeed { get { return data.dashSpeed; } }
    public float DashDuration { get { return data.dashDuration; } }
    #endregion
    #region Mechanics Variables
    public float CoyoteTimeDuration { get { return data.coyoteTimeDuration; } }
    public float ApexModifierDuration { get { return data.apexModifierDuration; } }
    public float ApexModifierMovementBoost { get { return data.apexModifierMovementBoost; } }
    #endregion

    public State currentState;
    public State previousState;

    private Transform2D _checkpointPosition;

    private Timer _teleportTimer;

    private Label velText;
    private Label stateText;
    private Label isOnFloorLabel;
    private Label floorNormalText;
    private Label floorAngleText;
    private Label dashCooldown;

    public override void _Ready()
    {
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");
        _teleportTimer = GetNode<Timer>("Teleport_Timer");

        velText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("Velocity_Label");
        stateText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("State_Label");
        isOnFloorLabel = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("IsOnFloor_Label");
        floorNormalText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorNormal_Label");
        floorAngleText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorAngle_Label");
        dashCooldown = GetNode<Label>("Label");
        ChangeState(new Falling(this));
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState.Execute(delta);

        velText.Text = "Velocity: " + Velocity.ToString();
        isOnFloorLabel.Text = "IsOnFloor: " + IsOnFloor();
        floorNormalText.Text = "FloorNormal: " + GetFloorNormal();
        floorAngleText.Text = "FloorAngle: " + GetFloorAngle();

        dashCooldown.Text = string.Format("{0:N0}", DashCooldownTimer.TimeLeft);
        MoveAndSlide();
    }

    public void ChangeState(State newState)
    {
        previousState = currentState;
        currentState = newState;

        previousState?.Exit();
        currentState.Enter();

        stateText.Text = "State: " + currentState.Name;
    }

    #region Communication with other game objects
    private void ChangeGravityProperties(Vector2 direction)
    {
        GD.Print("Gravity field entered");
        ChangeState(new Falling(this, direction));
    }

    private void ResetGravityProperties()
    {
        GD.Print("Gravity field left");
        ChangeState(new Falling(this));
    }

    private void ApplyJumpPadForce(Vector2 strength)
    {
        GD.Print("Jumppad entered");
        ChangeState(new Jumping(this, false, strength.Y));
    }

    private void Teleport(Vector2 newPos, Vector2 impulse)
    {
        GD.Print("Teleport entered");
        if(_teleportTimer.TimeLeft > 0) { return; }

        // Set the position to the teleport position
        Transform = new Transform2D(0, newPos);
        Velocity = Vector2.Zero;
        Velocity += impulse;

        _teleportTimer.Start();
    }

    private void SaveCheckpointLocation()
    {
        GD.Print("Checkpoint entered");
        _checkpointPosition = Transform;
    }

    public enum InteractionMode
    {
        Nothing = 0,
        GravityField,
        JumpPad,
        Teleport
    }
    #endregion
}
