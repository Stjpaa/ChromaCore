using Godot;
using System;

// counts up the amount of seconds a Level was played in total. 0 by default, but when a Level is reentered the timer gets set to its previous value 
public partial class LevelTimer : Control
{
    public double timeLevelWasPlayedInSeconds = 0;

    public override void _Process(double delta)
    {

        timeLevelWasPlayedInSeconds += delta;
    }
}
