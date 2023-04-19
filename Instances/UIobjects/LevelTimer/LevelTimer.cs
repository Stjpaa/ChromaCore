using Godot;
using System;

public partial class LevelTimer : Node2D
{
    public double timeLevelWasPlayedInSeconds = 0;


    public override void _Process(double delta)
    {
        timeLevelWasPlayedInSeconds += delta;
    }

    private void SaveTime(string pathToSavegame) 
    { 
    
    }
}
