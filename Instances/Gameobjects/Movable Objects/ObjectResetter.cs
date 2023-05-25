using Godot;
using System;

public partial class ObjectResetter : Area2D
{
	[Export]
	private RigidBody2D _gameObject;

	public void OnBodyEntered(Node2D body)
	{
		if (body is Box)
		{
			GD.Print("Trigger");
			body.Call("ResetPosition");
		}
	}
}
