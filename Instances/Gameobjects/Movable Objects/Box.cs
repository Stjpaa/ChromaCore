using Godot;
using System;

public partial class Box : CharacterBody2D
{
	private float _defaultGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private Vector2 _currentGravity;

	[Export]
	private Node2D gravityfieldList;

	private float mass = 50;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var children = gravityfieldList.GetChildren();
		foreach(var child in children)
		{
			child.Connect("OnGravityfieldEntered", new Callable(this, MethodName.ChangeGravityProperties));
			child.Connect("OnGravityfieldExited", new Callable(this, MethodName.ResetGravityProperties));
		}
		

		this._currentGravity = new Vector2(0, _defaultGravity);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		this.Velocity += this._currentGravity * (float)delta;

		if (this.IsOnFloor())
		{
			this.Velocity = new Vector2(this._currentGravity.X * 0.95f, this._currentGravity.Y) * (float)delta;
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
		this._currentGravity = new Vector2(0, this._defaultGravity);
	}
}
