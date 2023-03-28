using Godot;
using System;

public partial class GravityfieldSwitch : RigidBody2D
{
    private CollisionShape2D _gravityfieldCollider;

    public override void _Ready()
    {
        this._gravityfieldCollider = GetParent().GetNode<CollisionShape2D>("GravityfieldCollider");
    }

    // public void Init(Area2D parent)
    // {
    //     this._parentObject = parent;
    // }

    public void OnBodyEntered(Node body)
    {
        this._gravityfieldCollider.Disabled = true;
    }

    public void OnBodyExited(Node body)
    {
        this._gravityfieldCollider.Disabled = false;
    }
}
