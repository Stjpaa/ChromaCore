using Godot;
using System;

public partial class QuitButton : Button
{
    [Export] PauseMenu pauseMenu;

    public override void _Pressed()
    {
        pauseMenu.UnPauseGame();

        // Save Game When this Button is pressed 
        // maybe better to emit signal when this Button is pressed

        SaveSystem.BackToLevelSelectScreen(GetTree());
    }
}
