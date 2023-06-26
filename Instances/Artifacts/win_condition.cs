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
        GD.Print("Game Won");
    }


}
