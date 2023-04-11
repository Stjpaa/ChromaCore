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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var allGravityfields = this._gravityfieldList.GetChildren();
		var allGameobjects = this._gameObjectsList.GetChildren();
		var allJumpPads = this._jumpPadsList.GetChildren();
		var allPortals = this._portalsList.GetChildren();


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
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
