using Godot;
using System;
using System.Threading.Tasks;

/// <summary>
/// Used to change which Keyboard/ Controller Inputs move the player. Fore Example remap from wasd to esdf as movement.
/// </summary>
public partial class RemapInputs : Control
{
    private string actionName = "ui_up";

    private InputEvent keyboardOne;
    private InputEvent keyboardTwo;
    private InputEvent controllerOne;

    public override void _Ready()
    {
        GD.PrintErr("could cause Problems ith the order, because new actions would cause controllerOne to move up to [1] in the InputList should probalby safe new Button");

        UpdateCurrentInputevents(actionName);

        PrintAllInputs(actionName);

        InputEvent replacementEvent = controllerOne;
        ReplaceInputEvent(actionName, keyboardOne, replacementEvent);
        PrintAllInputs(actionName);
    }

    public override void _Process(double delta)
    {
        if(Input.IsAnythingPressed())
        {
            //RemapInputs ( pressedKey)
        }
    }

    private void UpdateCurrentInputevents(string action)
    {
        var currentInputEvents = InputMap.ActionGetEvents(action);

        keyboardOne = currentInputEvents[0];
        keyboardTwo = currentInputEvents[1];
        controllerOne = currentInputEvents[2];
    }

    private void PrintAllInputs(string action)
    {
        foreach (var input in InputMap.ActionGetEvents(action))
        {
            GD.Print(input.AsText());
        }
    }

    /// <summary>
    /// Removes a Event that is currently in the InputMap and Replaces it with a new one.
    /// 
    /// </summary>
    public void ReplaceInputEvent(string action,InputEvent eventToRemove ,InputEvent eventToAdd)    
    {
        //causes Problems if the same Input is assigned twice to the same action,
        //or if it is part of a different action (for example up and rigth are both assigned d -> causes Problem)




        InputMap.ActionEraseEvent(action, eventToRemove);
        InputMap.ActionAddEvent(action, eventToAdd);
        
    }

    public override void _Input(InputEvent inputEvent)
    {
        if(inputEvent is InputEventKey inputKey)
        {
            GD.Print(inputEvent.AsText());
        }

            //GD.Print(inputEvent.GetType());
        
        //GD.Print(inputEvent.AsText());
    }

    public void OnButtonPressedSignal()
    {
        
    }


    
}
