using Godot;
using System;

public partial class PauseMenu : Control
{
    
    [Export] private Control pauseMenuUI;
    [Export] private ColorRect blur;

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
                if(pauseMenuUI.Visible == false)    // while settings menu is open, dont unpause the game
                {
                    return;
                }
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

        Input.MouseMode = Input.MouseModeEnum.Hidden;

        pauseMenuUI.ProcessMode = ProcessModeEnum.Inherit;
        pauseMenuUI.Visible = true;    //Show Pause Menu
        blur.Visible = true;
    }

    public void UnPauseGame()
    {
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;

        pauseMenuUI.ProcessMode = ProcessModeEnum.Disabled;
        pauseMenuUI.Visible = false;     //Hide Pause Menu
        blur.Visible = false;
    }

    public void QuitGame()
    {
        LevelInstantiater instantiaterOfScene = (LevelInstantiater)GetTree().Root.GetNode("LevelInstantiater");

        if (instantiaterOfScene == null)
        {
            GD.PrintErr("No LevelInstanciater in Scene, Exit game instead.");
            GetTree().Quit();
            return;
        }

        _ = instantiaterOfScene.QuitLevelAsync();
    }

    public void TestSave()
    {
        LevelInstantiater instantiator = (LevelInstantiater)GetTree().Root.GetNode("LevelInstantiater");

        if (instantiator == null)
        {
            GD.PrintErr("No LevelInstantiater in Scene");
            return;
        }
        instantiator.SaveLevelVariables();


    }

    public override void _ExitTree()
    {
        UnPauseGame();
    }
}
