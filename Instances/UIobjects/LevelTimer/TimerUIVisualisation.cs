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
        double timeInSeconds = levelTimer.timeLevelWasPlayedInSeconds;
        //string timeFormatedAsText = "%02d : %02d : %04d" % [timeInSeconds,timeInSeconds,timeInSeconds];


        //levelTimer.timeLevelWasPlayedInSeconds;
        //Text = timeFormatedAsText;

        Text = "test";
        
    }

    
}
