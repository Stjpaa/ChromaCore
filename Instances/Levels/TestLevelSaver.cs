using Godot;
using System;

public partial class TestLevelSaver : Button
{
    private Node nodeToBeSaved;
    private const string _savePath = "saved_scene.tscn";

    public override void _Ready()
    {
        nodeToBeSaved = GetTree().Root;
        GD.Print(GetTree().Root.Name);
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

}
