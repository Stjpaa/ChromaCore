using Godot;
using System;
using PlayerController;

public partial class FollowingCamera : Camera2D
{
    public void UpdatePosition(Vector2 targetPosition)
    {
        var target = targetPosition;

        var targePosX = (int)(Mathf.Lerp(GlobalPosition.X, target.X, 0.9f * (float)GetProcessDeltaTime() * 100f));
        var targePosY = (int)(Mathf.Lerp(GlobalPosition.Y, target.Y, 0.9f * (float)GetProcessDeltaTime() * 100f));

        GlobalPosition = new Vector2(targePosX, targePosY);
        //GlobalPosition = targetPosition;
    }
}
