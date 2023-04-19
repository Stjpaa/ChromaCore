using Godot;
using System;

public partial class Level : Node2D
{
    [Export] public PackedScene baseLevelToLoad;
    [Export] public Node2D nodeWhichToPack;

    private bool levelCompleted = false;

    

    public void LoadLevel()
    {
        //SaveSystem.LoadLevel(this);
        SaveSystem.LoadLevelWithLevelInstantiator(this);
    }

    //public void DeleteSaveData()
    //{
    //    SaveSystem.DeleteSaveGameData(baseLevelToLoad);
    //}
}