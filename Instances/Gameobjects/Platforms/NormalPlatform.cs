using Godot;
using System;

[Tool]
public partial class NormalPlatform : StaticBody2D
{
	[Export]
	private Vector2 _collisionSize = new Vector2(16, 8);

	private CollisionShape2D _platformCollider;

	private Texture _sprite;

	public override void _Ready()
	{
		// Get dependencies
		this._platformCollider = GetNode<CollisionShape2D>("PlatformCollider");
		this._sprite = GetNode<Sprite2D>("Sprite").Texture;

		// Set collision- and sprite size
		this._platformCollider.Shape.Set("size", this._collisionSize);
		this._sprite.Set("width", this._platformCollider.Shape.GetRect().Size.Y);
		this._sprite.Set("height", this._platformCollider.Shape.GetRect().Size.X);
	}

	
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._platformCollider.Shape.Set("size", this._collisionSize);
			this._sprite.Set("width", this._platformCollider.Shape.GetRect().Size.Y);
			this._sprite.Set("height", this._platformCollider.Shape.GetRect().Size.X);
		}
	}
}
