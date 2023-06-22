using Godot;
using System;

public partial class InputsResource : Resource
{
    [Export] public InputEvent[] upInputEventArray = {null, null, null};    // Array has to have right starting size
    [Export] public InputEvent[] downInputEventArray = {null, null, null};    // Array has to have right starting size


}
