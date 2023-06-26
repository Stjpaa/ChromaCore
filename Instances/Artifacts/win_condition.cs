using Godot;
using System;
using System.Linq;

public partial class win_condition : Node2D
{
    private Artifact[] artifacts = { null, null, null, null, null, null, null };    // max size 
    private int IndexForNextArrayElement = 0;

    public void AddArtifactToArray(Artifact artifact)
    {
        artifacts[IndexForNextArrayElement] = artifact;
        IndexForNextArrayElement++;
    }

    public void CheckIfWon()
    {
        foreach (var artifact in artifacts)
        {
            if (artifact != null)
            {
                if (artifact.collected == false)
                {
                    return;     // not won yet
                }
            }
        }

        // if all were collected win game

        WinGame();
    }


    private void WinGame()
    {
        var winScreen = (CanvasLayer)GetNode("CanvasLayer");
        winScreen.Visible = true;

        GetTree().Paused = true;    // affects all Nodes that have the property: Node -> Mode -> Inherit .    this Node should be set to Always instead

        Input.MouseMode = Input.MouseModeEnum.Hidden;
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
        else
        {
            _ = instantiaterOfScene.QuitLevelAsync();
        }

    }
}
