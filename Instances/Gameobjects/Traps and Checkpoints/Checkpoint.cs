using Godot;
using System;

[Tool]
public partial class Checkpoint : Area2D
{
	[Export]
	private float _collisionWidth = 50f;

	private CollisionShape2D _collider;

	public override void _Ready() 
	{
		this._collider = GetNode<CollisionShape2D>("Collider");
		this._collider.Shape.Set("size", new Vector2(this._collisionWidth, 100));
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._collider.Shape.Set("size", new Vector2(this._collisionWidth, 100));
		}
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is PlayerController.PlayerController2D)
		{
			body.Call("SaveCheckpointLocation", this.GlobalPosition);
		}
	}
}
