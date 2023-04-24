using Godot;
using System;

public partial class TestLevelSaver : Button
{
    [Export] public LevelInstantiater LevelToSave;

    public override void _Pressed()
    {
        //if(LevelToSave == null)
        //{
        //    GD.PrintErr("no LevelInstantiater was assigned to TestLevelSaver");
        //    return;
        //}
        //LevelToSave.SaveLevel();
        SaveSystem.BackToLevelSelectScreen(GetTree());
    }


}
