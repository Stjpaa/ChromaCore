using Godot;
using System;

public partial class RagequitHandler : Node2D
{
    private double counter = 0;
    public override void _Ready()
    {
        GetTree().AutoAcceptQuit = false;


    }

    public override void _Process(double delta)
    {
        counter += delta;

        if(counter >= 5)
        {
            QuitGame();
        }

    }


    private void QuitGame()
    {
        GD.Print("quit game via RagequtHandler");
        GetTree().Quit();
    }


    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey)
        {
            if(((InputEventKey)@event).AsTextKeycode() == "Escape")
            {
                QuitGame();
            }
        }
    }

}
