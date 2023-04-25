using Godot;
using System;

public partial class GrapplingHookJointBased : Node
{
    public DampedSpringJoint2D joint { get; private set; }

    public RigidBody2D hookStart {  get; private set; }
    public RigidBody2D hookEnd { get; private set; }
  
    public override void _Ready()
    {
        joint = GetNode<DampedSpringJoint2D>("DSJ");
        hookStart = GetNode<RigidBody2D>("DSJ/HookStart");
        hookEnd = GetNode<RigidBody2D>("DSJ/HookEnd");
    }

    public void InitializeHook(Vector2 globalStartPos, Vector2 globalEndPos, int length)
    {
        SetHookLength(length);
        SetHookStartPosition(globalStartPos);
        SetHookRotation(globalStartPos, globalEndPos);
    }

    private void SetHookLength(int length)
    {
        joint.Length = length;
        hookEnd.Position = new Vector2(0, length);
    }

    private void SetHookStartPosition(Vector2 position)
    {
        joint.GlobalPosition = position;
    }

    private void SetHookRotation(Vector2 startPos, Vector2 endPos)
    {
        var angle = (endPos - startPos).Angle() - (float)Math.PI/2;
        joint.Rotation = angle;
    }
}
