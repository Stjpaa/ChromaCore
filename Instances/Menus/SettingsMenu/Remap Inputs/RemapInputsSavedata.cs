using Godot;
using System;

public partial class RemapInputsSavedata 
{
    //up, down, left, right, dash, jump, interact


    // either save the InputEvent itself, or cast it to and form string?
    public InputEvent upKeyboard1 { get; set; }
    

    public void LoadBaseInputMap()
    {
        upKeyboard1 = InputMap.ActionGetEvents("ui_up")[0];
        GD.Print("test + " + upKeyboard1.AsText());
    }
}
