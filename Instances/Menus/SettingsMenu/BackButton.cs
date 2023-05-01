using Godot;
using System;

public partial class BackButton : Button
{
    [Export] private SettingsMenu settingsMenu;

    public override void _Pressed()
    {
        settingsMenu.CloseSettingsMenu();
    }
}
