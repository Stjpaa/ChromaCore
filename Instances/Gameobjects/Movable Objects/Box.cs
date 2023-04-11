using Godot;
using System;

public partial class Box : RigidBody2D
{
	private Vector2 _defaultGravity = new Vector2(0, ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle());
	private Vector2 _currentGravity;

	private bool _enteredJumpPad = false;

	private Vector2 _jumpPadStrength;

	private bool _enteredPortal = false;

	private Vector2 _teleportPos;

	private Vector2 _teleportImpulse;

	private float _teleportTimer = 1f;

	public override void _Ready()
	{
		this._currentGravity = this._defaultGravity;
		LinearVelocity = this._currentGravity;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		state.LinearVelocity += this._currentGravity * (float)GetPhysicsProcessDeltaTime();
		this._teleportTimer += (float)GetPhysicsProcessDeltaTime();

		if (this._enteredJumpPad)
		{
			state.ApplyImpulse(this._jumpPadStrength);
			this._enteredJumpPad = false;
		}

		if (this._enteredPortal)
		{
			state.Transform = new Transform2D(0, this._teleportPos);
			state.LinearVelocity = new Vector2(0, 0);
			state.ApplyImpulse(this._teleportImpulse);
			this._enteredPortal = false;
			this._teleportTimer = 0;
		}
	}

	private void ChangeGravityProperties(Vector2 direction, float strength)
	{
		this._currentGravity = direction.Normalized() * strength;
	}

	public void ResetGravityProperties()
	{
		this._currentGravity = this._defaultGravity;
	}
	
	public void ApplyJumpPadForce(Vector2 strength)
	{
		this._enteredJumpPad = true;
		this._jumpPadStrength = strength;	
	}

	public void Teleport(Vector2 newPos, Vector2 impulse)
	{
		if (this._teleportTimer > 1f)
		{
			this._enteredPortal = true;
			this._teleportPos = newPos;
			this._teleportImpulse = impulse;
		}

	}
}
