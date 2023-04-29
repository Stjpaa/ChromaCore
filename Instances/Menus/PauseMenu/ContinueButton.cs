using Godot;
using System;

public partial class ContinueButton : Button
{
    [Export] PauseMenu pauseMenu;

    public override void _Pressed()
    {
        pauseMenu.UnPauseGame();
    }
}
