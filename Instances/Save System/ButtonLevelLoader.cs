using Godot;
using System;

public partial class ButtonLevelLoader : Button
{
    [Export] public Level levelToBeLoaded;


    public override void _Pressed()
    {
        levelToBeLoaded.LoadLevel();
    }

}
