using Godot;
using System;

public partial class Level : Button
{
    [Export] public PackedScene baseLevelToLoad;
    //[Export] public Node2D nodeWhichToPack;
    public LevelVariablesSaveData levelVariablesSaveData;



    public override void _Ready()
    {
        LoadSaveData();     // also called in LevelSelectVisualisation.Ready() because its needed before this _Ready() happens
    }

    public void LoadSaveData()
    {
        if (baseLevelToLoad == null)
        {
            levelVariablesSaveData = new LevelVariablesSaveData();
            return;
        }

        levelVariablesSaveData = SaveSystem.LoadLevelVariablesSaveData(baseLevelToLoad);

    }


    public override void _Pressed()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        //SaveSystem.LoadLevel(this);
        SaveSystem.LoadLevelWithLevelInstantiator(this);
    }



    private void DisplayLevelTime()
    {

    }
    //public void DeleteSaveData()
    //{
    //    SaveSystem.DeleteSaveGameData(baseLevelToLoad);
    //}
}
