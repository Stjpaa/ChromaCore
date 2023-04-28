using Godot;
using System;

public partial class LevelManager : Node2D
{
    private LevelVariablesSaveData levelVariablesOnLoad;   // set via LevelInstantiator

    [Export] public LevelTimer levelTimer;

    
    public void InstantiateValues(LevelVariablesSaveData saveData)
    {
        levelVariablesOnLoad = saveData;


        levelTimer.timeLevelWasPlayedInSeconds = levelVariablesOnLoad.levelTimerInSeconds;
    }

    public LevelVariablesSaveData CreateUpdatedLevelVariablesSaveData()
    {
        LevelVariablesSaveData updatedSaveData = levelVariablesOnLoad;    // keep everything that was not changed

        updatedSaveData.levelTimerInSeconds = levelTimer.timeLevelWasPlayedInSeconds;

        return updatedSaveData;
    }
}
