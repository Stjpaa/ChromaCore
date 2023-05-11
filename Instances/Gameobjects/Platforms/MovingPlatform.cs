using Godot;
using System;

[Tool]
public partial class MovingPlatform : AnimatableBody2D
{
	[Export]
	private Vector2 _collisionSize = new Vector2(16, 8);

	private CollisionShape2D _platformCollider;

	private Texture _sprite;
	private Animation animation;

	public override void _Ready()
	{
		// Get dependencies
		this._platformCollider = GetNode<CollisionShape2D>("PlatformCollider");
		this._sprite = GetNode<Sprite2D>("Sprite").Texture;

		// Set collision- and sprite size
		this._platformCollider.Shape.Set("size", this._collisionSize);
		this._sprite.Set("width", this._platformCollider.Shape.GetRect().Size.Y);
		this._sprite.Set("height", this._platformCollider.Shape.GetRect().Size.X);

		AnimationPlayer animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
		animation = animation_player.GetAnimation("Move");
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

	public void RotateAround(Vector2 center, float angle)
	{
		for(int i = 0; i < animation.TrackGetKeyCount(0); i++)
		{
			Vector2 position = (Vector2)animation.TrackGetKeyValue(0, i);
			Vector2 offset = position - center;
			offset = offset.Rotated(angle*Mathf.Pi/180.0f);
			position = center + offset;
			animation.TrackSetKeyValue(0, i, position);
		}
	}
}
