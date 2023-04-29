using Godot;
using System;

public partial class Checkpoint : Area2D
{
	public delegate void OnCheckPointReachedEventHandler(Vector2 location);

	public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			body.Call("SaveCheckpointLocation");
		}
	}
}
