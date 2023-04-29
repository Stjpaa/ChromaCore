using Godot;
using System;

public partial class ObjectResetter : Area2D
{
	[Export]
	private RigidBody2D _gameObject;

	public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			this._gameObject.Call("ResetPosition");
		}
	}
}
