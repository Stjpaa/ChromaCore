using Godot;
using System;

public partial class TimerUIVisualisation : Label
{
    [Export]public LevelTimer levelTimer;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        UpdateTimerUI();
    }
    private void UpdateTimerUI()
    {
        Text = TimeSpan.FromSeconds(levelTimer.timeLevelWasPlayedInSeconds).ToString(@"mm\:ss\:fff");
    }

    
}
