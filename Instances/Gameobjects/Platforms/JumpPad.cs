using Godot;
using System;

public partial class JumpPad : StaticBody2D
{
	[Export]
	private float _strength = 500f;

	[Signal]
	public delegate void OnJumpPadEnteredEventHandler(Vector2 strength);

	public void OnBodyEntered(Node2D body)
	{
		EmitSignal("OnJumpPadEntered", new Vector2(0, -this._strength));
	}
}
