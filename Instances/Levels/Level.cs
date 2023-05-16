using Godot;
using System;

public partial class Level : Button
{
    [Export] public PackedScene baseLevelToLoad;
    //[Export] public Node2D nodeWhichToPack;
    private LevelVariablesSaveData levelVariablesSaveData;

    private LevelSelectVisualisation dataVisualisation;



    public override void _Ready()
    {
        LoadSaveData();

        dataVisualisation = (LevelSelectVisualisation)GetNode("LevelSelectVisualisation");
        dataVisualisation.VisualizeData(levelVariablesSaveData);
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
