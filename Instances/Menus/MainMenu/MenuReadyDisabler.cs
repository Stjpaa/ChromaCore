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
    private Control[] Allmenus;


}
