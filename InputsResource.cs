using Godot;
using System;

public partial class InputsResource : Resource
{

    [Export] public int testInt = 0;

    [Export] public InputEvent inputEventTest;

    [Export] public int[] inputEventArray = {0, 1};
}
