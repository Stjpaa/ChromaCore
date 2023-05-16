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

        Input.MouseMode = Input.MouseModeEnum.Hidden;

        pauseMenuUI.ProcessMode = ProcessModeEnum.Inherit;
        pauseMenuUI.Visible = true;    //Show Pause Menu
    }

    public void UnPauseGame()
    {
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;

        pauseMenuUI.ProcessMode = ProcessModeEnum.Disabled;
        pauseMenuUI.Visible = false;     //Hide Pause Menu
    }

    public void QuitGame()
    {
        // return to main menu, or quit?

        SaveSystem.BackToLevelSelectScreen(GetTree());
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
