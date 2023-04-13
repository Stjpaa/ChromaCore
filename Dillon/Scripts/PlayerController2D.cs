using Godot;
using System;

public partial class PlayerController2D : CharacterBody2D
{
    // GitHub Commit logs Version 0.0.8  
    // Fixed coyote time
    // - implemented in the falling state
    // - in the previous version it worked by delaying the transition to the falling state
    // - now it works by checking if the jump got triggered during a short time perdiod during the falling state
    // Implemented edge detection

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
    public Node2D RayNodeTopLeft
    {
        get;
        private set;
    }
    public Node2D RayNodeTopRight
    {
        get;
        private set;
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
    public float JumpSpeed { get { return data.jumpSpeed; } }
    public float JumpEndModifier { get { return data.jumpEndModifier; } }
    #endregion
    #region Dashing State Variables
    public float DashSpeed { get { return data.dashSpeed; } }
    public float DashDuration { get { return data.dashDuration; } }
    #endregion
    #region Mechanics Variables
    public float CoyoteTimeDuration { get { return data.coyoteTimeDuration; } }
    public float ApexModifierDuration { get { return data.apexModifierDuration; } }
    #endregion

    public State currentState;
    public State previousState;

    private Label velText;
    private Label stateText;
    private Label isOnFloorLabel;
    private Label floorNormalText;
    private Label floorAngleText;
    private Label dashCooldown;
    private Line2D floorAngleLine;

    public override void _Ready()
    {
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");

        //RayNodeTopLeft = GetNode<Node2D>("RayTopLeft_Node2D");
        //RayNodeTopRight = GetNode<Node2D>("RayTopRight_Node2D");

        velText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("Velocity_Label");
        stateText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("State_Label");
        isOnFloorLabel = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("IsOnFloor_Label");
        floorNormalText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorNormal_Label");
        floorAngleText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorAngle_Label");
        floorAngleLine = GetNode<Line2D>("FloorAngle_Line2D");
        dashCooldown = GetNode<Label>("Label");
        ChangeState(new Falling(this));

        FloorStopOnSlope = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState.Execute(delta);

        floorAngleLine.ClearPoints();

        velText.Text = "Velocity: " + Velocity.ToString();
        isOnFloorLabel.Text = "IsOnFloor: " + IsOnFloor();
        floorNormalText.Text = "FloorNormal: " + GetFloorNormal();
        floorAngleText.Text = "FloorAngle: " + GetFloorAngle();
        floorAngleLine.AddPoint(GetPositionDelta());
        floorAngleLine.AddPoint(GetPositionDelta() + GetFloorNormal() * 100f);
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
}
