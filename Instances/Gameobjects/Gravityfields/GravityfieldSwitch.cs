using Godot;
using System;

public partial class GravityfieldSwitch : Area2D
{
    [Signal]
    public delegate void OnSwitchTriggeredEventHandler();

    [Signal]
    public delegate void OnSwitchLeftEventHandler();

    public void OnBodyEntered(Node2D body)
    {
        if (body.GetParent().Name == "GameObjects")
        {
            EmitSignal("OnSwitchTriggered");
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if (body.GetParent().Name == "GameObjects")
        {
            EmitSignal("OnSwitchLeft");
        }
    }
}
