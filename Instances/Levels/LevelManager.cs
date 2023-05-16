using Godot;
using System;

public partial class LevelManager : Node2D
{
    private LevelVariablesSaveData levelVariablesOnLoad;   // set via LevelInstantiator

    [Export] public LevelTimer levelTimer;

    
    public void InstantiateValues(LevelVariablesSaveData saveData)
    {
        levelVariablesOnLoad = saveData;

        if (levelTimer == null)
        {
            GD.PrintErr("Assign levelTimer in LevelManager");
            return;
        }
        levelTimer.timeLevelWasPlayedInSeconds = levelVariablesOnLoad.levelTimerInSeconds;
    }

    public LevelVariablesSaveData CreateUpdatedLevelVariablesSaveData()
    {
        LevelVariablesSaveData updatedSaveData = levelVariablesOnLoad;    // keep everything that was not changed

        GD.Print("timer: " +updatedSaveData.levelTimerInSeconds);
        updatedSaveData.levelTimerInSeconds = levelTimer.timeLevelWasPlayedInSeconds;

        return updatedSaveData;
    }
}
