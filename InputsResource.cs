using Godot;
using System;
using static RemapInputs;
using static System.Collections.Specialized.BitVector32;

public partial class InputsResource : Resource
{
    private const string pathToInputsResource = "user://InputsResource.tres";

    [Export] public InputEvent[] upInputEventArray = { null, null };    // Arrays have to have right starting size, otherwise they seemingly can not be saved
    [Export] public InputEvent[] downInputEventArray = { null, null };
    [Export] public InputEvent[] leftInputEventArray = { null, null };
    [Export] public InputEvent[] rightInputEventArray = { null, null };
    [Export] public InputEvent dashInputEvent = null;


    public static InputsResource LoadInputsResource()
    {
        //ResetSavedResource();

        InputsResource loadedResource = (InputsResource)ResourceLoader.Load(pathToInputsResource);


        if (loadedResource != null)
        {
            GD.Print("succesfully loaded");
            return loadedResource;
        }
        else      // if no Resource already exists instead Load the Base
        {
            loadedResource.LoadBaseInputEventsOnStart();
            return loadedResource;
        }

    }

    /// <summary>
    /// if there already exists an unwanted Savegame at the ResourcePath this function deletes that Savegame and creates a brand new one.
    /// </summary>
    private void ResetSavedResource()       
    {
        InputsResource loadedResource = new InputsResource();
        loadedResource.LoadBaseInputEventsOnStart();
        loadedResource.SaveResource();
    }

    private void LoadBaseInputEventsOnStart()   // Load the basic InputEvents when they werent changed 
    {
        for (int i = 0; i < 2; i++)
        {
            upInputEventArray[i] = InputMap.ActionGetEvents("Jump")[i];
            downInputEventArray[i] = InputMap.ActionGetEvents("ui_down")[i];
            leftInputEventArray[i] = InputMap.ActionGetEvents("Move_Left")[i];
            rightInputEventArray[i] = InputMap.ActionGetEvents("Move_Right")[i];
        }
        dashInputEvent = InputMap.ActionGetEvents("Dash")[0];
    }

    public void ReplaceInputMapAction(RemapedInputs inputType, int indexOfButton, InputEvent replacementInput)
    {
        if (IsKeyusedAnywhere(replacementInput.AsText()))
        {
            GD.PrintErr("Action " + replacementInput.AsText() + " is already assigned for another action");
            return;
        }

        switch (inputType) {
            case RemapedInputs.none:
                {
                    GD.PrintErr("no type was assigned on remap button");
                }
                break;

            case RemapedInputs.up:
                {
                    InputMap.ActionEraseEvent("ui_up", upInputEventArray[indexOfButton]);
                    InputMap.ActionEraseEvent("Jump", upInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_up", replacementInput);
                    InputMap.ActionAddEvent("Jump", replacementInput);

                    upInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change
                }
                break;

            case RemapedInputs.down:
                {
                    InputMap.ActionEraseEvent("ui_down", downInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_down", replacementInput);

                    downInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change
                }
                break;

            case RemapedInputs.left:
                {
                    InputMap.ActionEraseEvent("ui_left", leftInputEventArray[indexOfButton]);
                    InputMap.ActionEraseEvent("Move_Left", leftInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_left", replacementInput);
                    InputMap.ActionAddEvent("Move_Left", replacementInput);

                    leftInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change

                }
                break;

            case RemapedInputs.right:
                {
                    InputMap.ActionEraseEvent("ui_right", rightInputEventArray[indexOfButton]);
                    InputMap.ActionEraseEvent("Move_Right", rightInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_right", replacementInput);
                    InputMap.ActionAddEvent("Move_Right", replacementInput);

                    rightInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change

                }
                break;

            case RemapedInputs.dash:
                {
                    InputMap.ActionEraseEvent("Dash", dashInputEvent);
                    InputMap.ActionAddEvent("Dash", replacementInput);

                    dashInputEvent = replacementInput;
                    SaveResource();
                }
                break;
        }

    }


    public void ReplaceInputEvent(string action, InputEvent eventToRemove, InputEvent eventToAdd)
    {
        InputMap.ActionEraseEvent(action, eventToRemove);
        InputMap.ActionAddEvent(action, eventToAdd);
    }

    public void SaveResource()
    {
        ResourceSaver.Save(this, pathToInputsResource);
    }

    public void TestReplace()
    {
        GD.Print("key is used somewhere = " + IsKeyusedAnywhere("A (Physical)"));
    }






    private bool IsKeyusedAnywhere(string keyAsText)    // Checks all currently used buttons to prevent one Button being assigned to two different actions, ie left + right
    {
        foreach (var element in upInputEventArray)
        {
            if(element.AsText() == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in downInputEventArray)
        {
            if(element.AsText() == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in leftInputEventArray)
        {
            if (element.AsText() == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in rightInputEventArray)
        {
            if (element.AsText() == keyAsText)
            {
                return true;
            }
        }

        if(dashInputEvent.AsText() == keyAsText)
        {
            return true;
        }


        // Else this key is not currently used, so it can be assigned to the new action
        return false;
    }
}
