using Godot;
using System;

public partial class LevelInstantiater : Node2D
{
    public Level LevelToLoad;
    [Export] public Node2D LevelRoot;

    public void SaveLevel()
    {
        // Save Everything below the Node CurrentLevelRoot or whatever i decide on.
    }

    public void LoadLevel() 
    {
        // Load Level(LevelToLoad) and add it below a child Node
    }

}
