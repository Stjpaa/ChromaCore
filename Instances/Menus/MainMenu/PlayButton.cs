using Godot;
using System;

public partial class PlayButton : Button
{
    [Export] private PackedScene levelSelectScene;


    public override void _Pressed()
    {
        if(levelSelectScene == null)
        {
            GD.PrintErr("No Scene Assigned in PlayButton Skript");
            return;
        }

        GetTree().ChangeSceneToPacked(levelSelectScene);
    }

}
