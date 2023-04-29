using Godot;
using System;

public partial class Traps : Area2D
{
	public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			body.Call("Respawn");
		}
	}
}
