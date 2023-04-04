using Godot;
using System;

public partial class InstantiateLevel : Node2D
{
	[Export]
	private Node _gravityfieldList;

	[Export]
	private Node _gameObjects;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var allGravityfields = this._gravityfieldList.GetChildren();
		var allGameobjects = this._gameObjects.GetChildren();

		foreach(var gameobject in allGameobjects)
		{
			foreach(var gravityfield in allGravityfields)
			{
				try 
				{
					gravityfield.Connect("OnGravityfieldEntered", new Callable(gameobject, "ChangeGravityProperties"));
					gravityfield.Connect("OnGravityfieldExited", new Callable(gameobject, "ResetGravityProperties"));
				}
				catch (Exception e)
				{
					GD.Print("Error while connecting gameobjects to gravityfields: " + e.ToString());
				}

			}
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
