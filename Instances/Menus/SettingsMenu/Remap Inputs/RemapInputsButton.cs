using Godot;
using System;

public partial class RemapInputsButton : Button
{
    [Export] public RemapInputs.RemapedInputs inputToRemapType;
    [Export] public int positionInRemapArray = 0;
    [Export] private RemapInputs remapInputs;

    public override void _Ready()
    {
        Text = remapInputs.GetTextForButton(this);
    }

    public override void _Pressed()
    {
        remapInputs.OnButtonPressed(this);
    }

    //public void UpdateText()
    //{
    //    string v = RemapInputs.GetTextForButton(this);
    //    Text = v;
    //}


}
