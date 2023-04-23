using Godot;
using System;

public partial class PlayButton : Button
{
    [Export] private PackedScene levelSelectScene;

    public override void _Ready()
    {
        GrabFocus();    // be the first selected UI Element
    }

    public override void _Pressed()
    {
        if(levelSelectScene == null)
        {
            GD.PrintErr("LevelselectScene was not assigned in the Playbutton Script");
            return;
        }

        GetTree().ChangeSceneToPacked(levelSelectScene);
    }
}
