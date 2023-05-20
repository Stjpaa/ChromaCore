using Godot;
using System;
using System.ComponentModel;

public partial class PlayerController2D_Data : Resource
{    
	[ExportCategory("Moving State")]
	[Export]
	public float maxMoveSpeed = 250f;
	[Export]
	public float moveSpeedAcceleration = 40f;
	[Export]
	public float moveSpeedDecceleration = 40f;


	[ExportCategory("Falling State")]
	[Export]
	public float maxFallSpeed = 700f;
	[Export]
	public float fallSpeedAcceleration = 20f;
	[Export]
	public float maxFallingMoveSpeed = 225f;
	[Export]
	public float fallingMoveSpeedAcceleration = 32f;
	[Export]
	public float FallingMoveSpeedDeceleration = 10f;


	[ExportCategory("Jumping State")]
	[Export]
	public float jumpForce = 600f;
	[Export]
	public float jumpEndModifier = 10f;

	[ExportCategory("Dashing State")]
	[Export]
	public float dashSpeed = 1000f;
	[Export]
	public float dashDuration = 0.2f;

	[ExportCategory("Mechanics")]
	[Export]
	public float coyoteTimeDuration = 2f;
	[Export]
	public float apexModifierDuration = 0.05f;
	[Export]
	public float apexModifierMovementBoost = 2f;
}
