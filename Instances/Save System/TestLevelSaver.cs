using Godot;
using System;

public partial class TestLevelSaver : Button
{
    [Export] public Level LevelToSave;
    public override void _Ready()
    {
    }

    public override void _Pressed()
    {
        LevelToSave.SaveLevel();
    }


}
