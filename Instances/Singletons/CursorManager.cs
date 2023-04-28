using Godot;
using System;

public partial class CursorManager : Node2D
{


    public override void _Process(double delta)
    {
        //if (Input.GetLastMouseSpeed() == Vector2.Zero)
        {
            // Hide the cursor
            GetTree().Root.Visible = false;
        }
    }

    private void HideCursor()
    {
        GetTree().Root.Visible = false;
    }

    private void ShowCursor()
    {
        GetTree().Root.Visible = true;
    }

}
