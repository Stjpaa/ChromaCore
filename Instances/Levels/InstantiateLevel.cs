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
		GD.Print("_gravityfieldList = " + _gravityfieldList + "\n_gameObjects = " + _gameObjects);

		var allGravityfields = this._gravityfieldList.GetChildren();
		var allGameobjects = this._gameObjects.GetChildren();

		foreach(var gravityfield in allGravityfields)
		{
			foreach(var gameobject in allGameobjects)
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


	public string LoadOnInstantiation()
	{
		_gravityfieldList = GetChild(4);
		_gameObjects = GetChild(3);


		GD.PrintErr("test test, load levelinfo");
		return "test callback";
	}

	public void SaveOnInstantiation()
	{
		GD.PrintErr(_gravityfieldList.GetPath());	// would need to be adjusted, because path would on load not contain LevelRoot and Instanciator		

    }
}
