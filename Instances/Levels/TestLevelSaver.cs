using Godot;
using System;

public partial class TestLevelSaver : Node2D
{
    public Node nodeToBeSaved;
    private const string _savePath = "user://saved_scene.tscn";

    public override void _Ready()
    {
        nodeToBeSaved = GetTree().Root;
        GD.Print(GetTree().Root.Name);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Key1)
            {
                GD.Print("1 pressed: save game");
                SaveScene();
            }
            if (eventKey.Pressed && eventKey.Keycode == Key.Key2)
            {
                GD.Print("2 pressed: load game");
                LoadScene();
            }
        }
    }

    public void SaveScene()
    {
        // Save the scene as a packed scene file
        ResourceSaver.Save(ScenePacker.CreatePackage(nodeToBeSaved), _savePath);
    }

    public void LoadScene()
    {
        // Load the saved scene from disk
        var savedScene = (PackedScene)ResourceLoader.Load(_savePath);

        // Replace the current scene with the loaded scene
        GetTree().ChangeSceneToPacked(savedScene);//ChangeSceneTo(savedScene);
    }

    public override void _ExitTree()
    {
        //SaveScene();
    }
}
