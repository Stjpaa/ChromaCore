using Godot;
using System;

[Tool]
public partial class JumpPad : StaticBody2D
{
	[Export]
	private Vector2 _objectCollisionSize = new Vector2(16, 8);

	[Export]
	private float _strength = 500f;

	[Export]
	private AudioStreamPlayer2D _audioPlayer;

	[Signal]
	public delegate void OnJumpPadEnteredEventHandler(Vector2 strength);

	private CollisionShape2D _objectCollider;

	private CollisionShape2D _jumpCollider;

	private Texture _sprite;

	public override void _Ready()
	{
		// Get dependencies
		this._objectCollider = GetNode<CollisionShape2D>("ObjectCollider");
		this._jumpCollider = GetNode<CollisionShape2D>("Area2D/JumpCollider");
		this._sprite = GetNode<Sprite2D>("Sprite").Texture;

		// Set collider- and sprite size
		this._objectCollider.Shape.Set("size", this._objectCollisionSize);
		this._jumpCollider.Shape.Set("size", new Vector2(this._objectCollisionSize.X - 2, 1));
		this._sprite.Set("width", this._objectCollider.Shape.GetRect().Size.Y);
		this._sprite.Set("height", this._objectCollider.Shape.GetRect().Size.X);
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._objectCollider.Shape.Set("size", this._objectCollisionSize);
			this._jumpCollider.Shape.Set("size", new Vector2(this._objectCollisionSize.X - 2, 1));
			this._sprite.Set("width", this._objectCollider.Shape.GetRect().Size.Y);
			this._sprite.Set("height", this._objectCollider.Shape.GetRect().Size.X);
		}
		
	}

	public void OnBodyEntered(Node2D body)
	{
		body.Call("ApplyJumpPadForce", new Vector2(0, -this._strength));
		_audioPlayer.Play();
	}
}
