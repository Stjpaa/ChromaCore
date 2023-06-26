using Godot;
using PlayerController;
using System;

public partial class RemapInputsButton : Button
{
    [Export] public RemapInputs.RemapedInputs inputToRemapType;
    [Export] public int positionInRemapArray = 0;
    [Export] private RemapInputs remapInputs;

    public override void _Ready()
    {
        UpdateButtonText();

        var callable = new Callable(this, "UpdateButtonText");
        remapInputs.Connect("UpdateAllButtonText", callable);
    }

    public override void _Pressed()
    {
        remapInputs.OnButtonPressed(this);
    }

    public void UpdateButtonText()
    {
        Text = remapInputs.GetTextForButton(this);
    }
}
