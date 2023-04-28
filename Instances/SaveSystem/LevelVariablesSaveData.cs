using Godot;
using System;

public partial class LevelVariablesSaveData 
{
    public double levelTimerInSeconds{ get; set; }
    public bool levelCompleted { get; set; }

    public double bestCompletionTime { get; set; }
}
