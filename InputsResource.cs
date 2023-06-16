using Godot;
using System;

public partial class InputsResource : Resource
{

    [Export] public int testInt = 0;

    [Export] public InputEvent testInputEvent;

    [Export] public int[] intArray = {0,0};

    [Export] public InputEvent[] inputEventArray = {null, null};    // Array has to have right starting size


}
