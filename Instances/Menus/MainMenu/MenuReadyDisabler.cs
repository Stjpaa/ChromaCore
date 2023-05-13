using Godot;
using System;

/// <summary>
/// disables all but the first menu that should be active. 
/// In Godot Nodes still Trigger _Process even when they are disabled, for UI Elements it in my opinion makes more Sense to disable all inactive Menus entirely
/// because otherwise the UINavigationManager of each menu would trigger each process and 
/// </summary>
public partial class MenuReadyDisabler : Control
{
    [Export] private Control firstActiveMenu;


    public override void _Ready()
    {
        DisableAllMenus();

        ActivateFirstMenu();
    }

    private void ActivateFirstMenu()
    {
        firstActiveMenu.Visible = true;
        firstActiveMenu.ProcessMode = ProcessModeEnum.Inherit;
    }

    private void DisableAllMenus()
    {
        foreach (var child in GetChildren())
        {
            Control childAsControl = child as Control;
            if (childAsControl != null)
            {
                childAsControl.Visible = false;
                childAsControl.ProcessMode = ProcessModeEnum.Disabled;
            }
        }
    }
}
