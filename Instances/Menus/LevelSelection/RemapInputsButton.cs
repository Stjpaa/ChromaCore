using Godot;
using System;

public partial class RemapInputsButton : Button
{
    private string actionName = "ui_up";
    [Export] private RemapInputs remapInputs;

    public override void _Pressed()
    {
        remapInputs.OnButtonPressed(this);
    }
}
