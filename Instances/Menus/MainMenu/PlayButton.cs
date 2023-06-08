using Godot;
using System;

public partial class PlayButton : Button
{
    [Export] private PackedScene levelSelectScene;


    public override void _Pressed()
    {
        if(levelSelectScene == null)
        {
            GD.PrintErr("LevelselectScene was not assigned in the Playbutton Script");
            return;
        }

        GetTree().ChangeSceneToPacked(levelSelectScene);
    }

    public override void _Process(double delta)
    {
        //if (IsHovered())          // test to see if UINavigationManager works (the Block Panel)
        //{
        //    GD.Print("hovered");
        //}
    }
}
