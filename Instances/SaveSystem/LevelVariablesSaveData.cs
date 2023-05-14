using Godot;
using System;

public partial class LevelVariablesSaveData 
{
    public double levelTimerInSeconds { get; set; } = 0;
    public bool levelCompleted { get; set; } = false;

    public double bestCompletionTime { get; set; } = -1;
}
