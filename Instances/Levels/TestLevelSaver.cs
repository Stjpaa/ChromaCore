using Godot;
using System;

public partial class TestLevelSaver : Button
{
    private string pathToFile = "res://Instances/Save System/LevelSelectScene.tscn";

    public override void _Ready()
    {
    }

    public override void _Pressed()
    {
        GD.Print("pressed");
        SaveScene();
        LoadLevelSelectScene();
    }


    public void SaveScene()
    {
        //Save the scene as a packed scene


        SaveSystem.SaveScene(this);
    }

    public void LoadLevelSelectScene()
    {
        SaveSystem.LoadSceneByPath(pathToFile, GetTree());
    }

}
