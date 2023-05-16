using Godot;
using System;

public partial class LevelSelectVisualisation : Label
{
    public void VisualizeData(LevelVariablesSaveData saveData)
    {
        this.Text = "Time Level was Played: " + TimeSpan.FromSeconds(saveData.levelTimerInSeconds).ToString(@"mm\:ss\:fff");
    }

    

}
