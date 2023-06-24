using Godot;
using System;

public partial class RemapInputsButton : Button
{
    [Export] public RemapInputs.RemapedInputs inputToRemapType;
    [Export] public int positionInRemapArray = 0;
    [Export] private RemapInputs remapInputs;

    public override void _Ready()
    {
        UpdateButtonText();
    }

    public override void _Pressed()
    {
        remapInputs.OnButtonPressed(this);
    }

    public void UpdateButtonText()
    {
        Text = remapInputs.GetTextForButton(this);
    }

    //public void UpdateText()
    //{
    //    string v = RemapInputs.GetTextForButton(this);
    //    Text = v;
    //}


}
