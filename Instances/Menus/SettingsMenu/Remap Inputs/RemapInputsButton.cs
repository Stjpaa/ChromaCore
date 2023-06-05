using Godot;
using System;

public partial class RemapInputsButton : Button
{
    [Export] public RemapInputs.RemapedInputs inputToRemapType;
    [Export] public int positionInRemapArray = 0;
    [Export] private RemapInputs remapInputs;
    [Export] public string actionName = "ui_up";

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
