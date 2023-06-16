using Godot;
using System;

public partial class SettingsButton : Button
{
    [Export] private Control menuOfThisButton;
    [Export] private SettingsMenu settingsMenu;

    public override void _Pressed()
    {
        settingsMenu.EnableMenu();
        settingsMenu.menuToReturnTo = menuOfThisButton;


        menuOfThisButton.Visible = false;
        menuOfThisButton.ProcessMode = ProcessModeEnum.Disabled;
    }
}
