using Godot;
using System;

public partial class CursorManager : Node2D
{


    public override void _Process(double delta)
    {
        //if (Input.GetLastMouseSpeed() == Vector2.Zero)
        {
            // Hide the cursor
            Input.MouseMode = Input.MouseModeEnum.Hidden;
        }
    }

    private void HideCursor()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    private void ShowCursor()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

}
