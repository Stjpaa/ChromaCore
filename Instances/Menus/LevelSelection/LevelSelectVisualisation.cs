using Godot;
using System;

public partial class LevelSelectVisualisation : Label
{
    [Export] private Level level;
    public override void _Ready()
    {
        level.LoadSaveData();       // sadly this causes the LevelData to Load twice, because the Level also Loads it in Ready, but this is the only way it makes sense in my opinion, because This happens before Level.Ready is called

        this.Text = "Time Level was Played: " + level.levelVariablesSaveData.levelTimerInSeconds + " seconds";
    }



}
