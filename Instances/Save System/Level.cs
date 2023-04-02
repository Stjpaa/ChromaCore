using Godot;
using System;

public partial class Level : Node2D
{
    [Export] public PackedScene baseLevel;

    private bool levelCompleted = false;

    public override void _Ready()
    {
        if (baseLevel != null)
        {
            if (SaveSystem.DoesSavegameExistFor(baseLevel))
            {
                GD.Print("Savegame does exist.");
            }
            else 
            { 
                GD.Print("Savegame does not yet exist"); 
            }
        }
    }


}
