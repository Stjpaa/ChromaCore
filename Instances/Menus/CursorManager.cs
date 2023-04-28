using Godot;
using System;

public partial class CursorManager: Node2D
{


    public override _
    if(Input.GetLastMouseSpeed() == Vector2.Zero)
    {
        // Hide the cursor
        GetTree().Root.Visible = false;
    }
}
