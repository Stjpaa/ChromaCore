using Godot;
using System;

public partial class PauseMenu : Control
{
    [Export] private Control pauseMenuUI;

    public override void _Ready()
    {
        if (GetTree().Paused)
        {
            PauseGame();    
        }
        else
        {
            UnPauseGame();
        }
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            //// also unpause when continue/quit gets pressed

            if (GetTree().Paused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()        
    {
        GetTree().Paused = true;    // affects all Nodes that have the property: Node -> Mode -> Inherit .    this Node should be set to Always instead

        pauseMenuUI.Visible = true;    //Show Pause Menu
    }

    public void UnPauseGame()
    {
        GetTree().Paused = false;
        pauseMenuUI.Visible = false;     //Hide Pause Menu
    }

}
