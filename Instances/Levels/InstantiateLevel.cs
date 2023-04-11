using Godot;
using System;

public partial class InstantiateLevel : Node2D
{
	[Export]
	private Node _gravityfieldList;

	[Export]
	private Node _gameObjectsList;

	[Export]
	private Node _jumpPadsList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("_gravityfieldList = " + _gravityfieldList + "\n_gameObjects = " + _gameObjectsList);

		var allGravityfields = this._gravityfieldList.GetChildren();
		var allGameobjects = this._gameObjectsList.GetChildren();
		var allJumpPads = this._jumpPadsList.GetChildren();

		foreach(var gameobject in allGameobjects)
		{
			foreach(var gravityfield in allGravityfields)
			{
				gravityfield.Connect("OnGravityfieldEntered", new Callable(gameobject, "ChangeGravityProperties"));
				gravityfield.Connect("OnGravityfieldExited", new Callable(gameobject, "ResetGravityProperties"));
				GD.Print("Connected " + gravityfield.Name + " to " + gameobject.Name);
			}

			foreach(var jumpPad in allJumpPads)
			{
				jumpPad.Connect("OnJumpPadEntered", new Callable(gameobject, "ApplyJumpPadForce"));
				GD.Print("Connected " + jumpPad.Name + " to " + gameobject.Name);
			}
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
