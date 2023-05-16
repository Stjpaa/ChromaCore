using Godot;
using System;

public partial class Level : Control
{
    [Export] public PackedScene baseLevelToLoad;
    //[Export] public Node2D nodeWhichToPack;
    private LevelVariablesSaveData levelVariablesSaveData;

    [Export]private LevelSelectVisualisation dataVisualisation;



    public override void _Ready()
    {
        UpdateValues();
    }

    private void UpdateValues()
    {
        LoadSaveData();

        DisplayLevelTime();
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


    public void LoadLevel()
    {
        SaveSystem.LoadLevelWithLevelInstantiator(this);

    }



    private void DisplayLevelTime()
    {
        dataVisualisation.VisualizeData(levelVariablesSaveData);
    }

    public void DeleteSaveData()
    {
        SaveSystem.DeleteLevelVariableSaveData(baseLevelToLoad);

        UpdateValues();
    }
}
