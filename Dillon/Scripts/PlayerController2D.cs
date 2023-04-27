using Godot;
using System;

public partial class PlayerController2D : CharacterBody2D
{
	// GitHub Commit logs Version 0.0.9
	// Refactoring

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

	public override void _Ready()
	{
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");

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
}
