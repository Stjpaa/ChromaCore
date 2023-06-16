using Godot;
using System;

public partial class LoadMainMenu : Button
{
    [Export] private string pathToMainMenu;
    public override void _Pressed()
    {
        GetTree().ChangeSceneToFile(pathToMainMenu);
    }
}
