using Godot;
using System;

public partial class Box : RigidBody2D
{
	private Vector2 _defaultGravity = new Vector2(0, ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle());
	private Vector2 _currentGravity;

	private PhysicsDirectBodyState2D _state;

	public override void _Ready()
	{
		this._currentGravity = this._defaultGravity;
		LinearVelocity = this._currentGravity;
	}

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
		state.LinearVelocity += this._currentGravity * (float)GetPhysicsProcessDeltaTime();
    }

	// public override void _PhysicsProcess(double delta)
	// {
	// 	this.Velocity += this._currentGravity * (float)delta;

	// 	if (this.IsOnFloorOnly())
	// 	{
	// 		// This somehow stops the box from sliding lol
	// 		this.Velocity = this._currentGravity * (float)delta;
	// 	}

	// 	MoveAndSlide();
	// }

	private void ChangeGravityProperties(Vector2 direction, float strength)
	{
		this._currentGravity = direction.Normalized() * strength;
	}

	public void ResetGravityProperties()
	{
		this._currentGravity = this._defaultGravity;
	}
}
