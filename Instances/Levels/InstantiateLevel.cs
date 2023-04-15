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

	[Export]
	private Node _portalsList;

	[Export]
	private Node _checkpointsList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("_gravityfieldList = " + _gravityfieldList + "\n_gameObjects = " + _gameObjectsList);

		// Get mechanic objects from the lists
		var allGravityfields = this._gravityfieldList.GetChildren();
		var allGameobjects = this._gameObjectsList.GetChildren();
		var allJumpPads = this._jumpPadsList.GetChildren();
		var allPortals = this._portalsList.GetChildren();
		var allCheckpoints = this._checkpointsList.GetChildren();

		// Connect mechanic objects to game objects such as Player, Boxes etc
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

			foreach(var portal in allPortals)
			{
				portal.Connect("TeleportObject", new Callable(gameobject, "Teleport"));
				GD.Print("Connected " + portal.Name + " to " + gameobject.Name);
			}

			if (gameobject.Name == "CharacterBody2D")
			{
				foreach(var checkpoint in allCheckpoints)
				{
					checkpoint.Connect("OnCheckpointReached", new Callable(gameobject, "SaveCheckpointLocation"));
					GD.Print("Connected " + checkpoint.Name + " to " + gameobject.Name);
				}
			}

		}
		
	}
}
