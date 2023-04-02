using Godot;
using System;

public partial class Level : Node2D
{
    [Export] public PackedScene baseLevel;
    [Export] public Node2D nodeWhichToPack;

    private bool levelCompleted = false;

    public void LoadLevel()
    {
        SaveSystem.LoadLevel(this);
    }

    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    } 
}
