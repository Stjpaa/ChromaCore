using Godot;
using System;

public partial class PlayerController_2D : CharacterBody2D
{   
    // GitHub Commit logs Version 0.0.6
    // - Prevented dash during falling if no direction is given
    // - Implemented coyote time
    // - Implemented dash cooldown

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
        dashCooldown.Text = String.Format("{0:N0}", DashCooldownTimer.TimeLeft);
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