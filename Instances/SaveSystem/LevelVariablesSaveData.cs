using Godot;
using System;

public partial class LevelVariablesSaveData 
{
    public string planetTexturePath { get; set; } = "res://Assets/Planets/CompletedPlanets/planet00.png";   // path to the base Planet might need to be adjusted 
    public double levelTimerInSeconds { get; set; } = 0;
    public bool levelCompleted { get; set; } = false;

    public double bestCompletionTime { get; set; } = -1;
}
