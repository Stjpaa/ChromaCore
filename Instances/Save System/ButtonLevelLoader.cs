using Godot;
using System;

public partial class ButtonLevelLoader : Button
{
    [Export] public PackedScene PackedScene;
    [Export] public int testInt = 4;

    public override void _Pressed()
    {
        LoadLevelOnPress();
    }


    private void LoadLevelOnPress()
    {
        PackedSceneLoader.LoadScene("res://Instances/Levels/Level-Prototype.tscn", GetTree());
    }
}
