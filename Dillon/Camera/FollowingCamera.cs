using Godot;
using System;

public partial class FollowingCamera : Camera2D
{
    private const float DEAD_ZONE = 160;

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion)
        {
            var target = GetGlobalMousePosition() - GetViewportRect().Size / 2f;
            if(target.Length() < DEAD_ZONE)
            {
                Position = Vector2.Zero;
            }
            else
            {
                Position = target.Normalized() * (target.Length() - DEAD_ZONE) * 0.25f;
            }
        }
    }
}
