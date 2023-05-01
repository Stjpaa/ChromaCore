using Godot;
using System;

public partial class Teststage : Node2D
{
    [Export]
    public PackedScene newRope;
    public Vector2 startPos = Vector2.Zero;
    public Vector2 endPos = Vector2.Zero;

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton && !@event.IsPressed())
        {
            if(startPos == Vector2.Zero)
            {
                startPos = GetGlobalMousePosition();
            }
            else if(endPos == Vector2.Zero)
            {
                endPos = GetGlobalMousePosition();
            }
            var rope = newRope.Instantiate() as Rope;
            AddChild(rope);
            rope.SpawnRope(startPos, endPos);
            startPos = Vector2.Zero;
            endPos = Vector2.Zero;
        }
    }
}
