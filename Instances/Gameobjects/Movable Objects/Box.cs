using Godot;
using System;

public partial class Box : CharacterBody2D
{
	private Vector2 _defaultGravity = new Vector2(0, ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle());
	private Vector2 _currentGravity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._currentGravity = this._defaultGravity;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		this.Velocity += this._currentGravity * (float)delta;

		if (this.IsOnFloorOnly())
		{
			// This somehow stops the box from sliding lol
			this.Velocity = this._currentGravity * (float)delta;
		}

		MoveAndSlide();
	}

	private void ChangeGravityProperties(Vector2 direction, float strength)
	{
		this._currentGravity = direction.Normalized() * strength;
		GD.Print(direction, strength);
	}

	public void ResetGravityProperties()
	{
		this._currentGravity = this._defaultGravity;
	}
}
