using Godot;
using System;

public partial class FollowingCamera : Camera2D
{
    [Export]
    public Node2D target = null;
    [Export]
    public float followingSpeed = 1.0f;

    public override void _Process(double delta)
    {
        System.Numerics.Vector2 cameraVector = new System.Numerics.Vector2(GlobalPosition.X, GlobalPosition.Y);
        System.Numerics.Vector2 targetVector = new System.Numerics.Vector2(target.GlobalPosition.X, target.GlobalPosition.Y);

        cameraVector = System.Numerics.Vector2.Lerp(cameraVector, targetVector, followingSpeed * (float)delta);

        GlobalPosition = new Vector2(cameraVector.X, cameraVector.Y);
    }
}
