using Godot;
using System;

public partial class ButtonDeleteScene : Button
{
    [Export] private Level level;

    public override void _Pressed()
    {
        level.DeleteSaveData();
    }
}
