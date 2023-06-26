using Godot;
using System;
using System.Linq;
using System.Threading;

public partial class Artifact : Area2D
{
    private win_condition winCon;
    
    public bool collected = false;

    public override void _Ready()
    {
        var callable = new Callable(this, "OnAreaEntered");
        Connect("body_entered", callable);

        winCon = (win_condition)GetParent();
        winCon.AddArtifactToArray(this);
    }

    public void OnAreaEntered(Node body)
    {
        if (collected)
        {
            return;
        }

        CollectArtifact();
        SetInactive();
    }

    public void CollectArtifact()
    {
        collected = true;
        winCon.CheckIfWon();
    }

    private void SetInactive()
    {
        this.Visible = false;
    }


}
