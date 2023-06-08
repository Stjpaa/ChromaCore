using Godot;
using System;
using System.IO;

public partial class TestInput : Node2D
{
    string inputToString;
    InputEventKey inputEvents;
    
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey inputKey)
        {
            inputToString = inputKey.AsTextKeycode();
            GD.Print("real Input: " + inputToString);

            ResourceSaver.Save(inputKey);
            inputEvents = new InputEventKey();
            
            //inputEvent._Set(inputToString);
            //GD.Print("copy Input: " + inputEvents.AsTextKeycode());
        }
    }
}

