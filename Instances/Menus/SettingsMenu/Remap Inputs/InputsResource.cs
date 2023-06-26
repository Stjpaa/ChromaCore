using Godot;
using System;
using System.Linq;
using static RemapInputs;
using static System.Net.Mime.MediaTypeNames;

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
        InputsResource loadedResource;
        if (SaveSystem.DoesFileExistAtPath(pathToInputsResource))
        {
            loadedResource = (InputsResource)ResourceLoader.Load(pathToInputsResource);
            ReassignLoadedValuesToMap(loadedResource);
        }
        else      // if no Resource exists instead Load the Base
        {
            loadedResource = new InputsResource();
            loadedResource.LoadBaseInputEventsOnStart();
            loadedResource.SaveResource();
        }

        return loadedResource;
    }

    /// <summary>
    /// compares the loaded values to the baseInputsResource values and then updates the Inputmap to make it match the loaded values.
    /// </summary>
    /// <param name="loadedResource"></param>
    private static void ReassignLoadedValuesToMap(InputsResource loadedResource)
    {
        BaseInputsResource baseInputsResource;
        baseInputsResource = BaseInputsResource.LoadBaseInputResource();

        GD.Print(baseInputsResource.upInputEventArray[0].AsText());
        GD.Print(InputMap.ActionGetEvents("ui_up")[0] == baseInputsResource.upInputEventArray[0]);
        InputMap.ActionEraseEvent("ui_up", (InputEventKey)baseInputsResource.upInputEventArray[0]);


        for (int i = 0; i <= 1; i++)
        {
            InputMap.ActionEraseEvent("ui_up", baseInputsResource.upInputEventArray[i]);
            InputMap.ActionEraseEvent("Jump", baseInputsResource.upInputEventArray[i]);

            InputMap.ActionAddEvent("ui_up", loadedResource.upInputEventArray[i]);
            InputMap.ActionAddEvent("Jump", loadedResource.upInputEventArray[i]);


            InputMap.ActionEraseEvent("ui_down", baseInputsResource.downInputEventArray[i]);
            InputMap.ActionAddEvent("ui_down", loadedResource.downInputEventArray[i]);


            InputMap.ActionEraseEvent("ui_left", baseInputsResource.leftInputEventArray[i]);
            InputMap.ActionEraseEvent("Move_Left", baseInputsResource.leftInputEventArray[i]);

            InputMap.ActionAddEvent("ui_left", loadedResource.leftInputEventArray[i]);
            InputMap.ActionAddEvent("Move_Left", loadedResource.leftInputEventArray[i]);


            InputMap.ActionEraseEvent("ui_right", baseInputsResource.rightInputEventArray[i]);
            InputMap.ActionEraseEvent("Move_Right", baseInputsResource.rightInputEventArray[i]);

            InputMap.ActionAddEvent("ui_right", loadedResource.rightInputEventArray[i]);
            InputMap.ActionAddEvent("Move_Right", loadedResource.rightInputEventArray[i]);

        }

        InputMap.ActionEraseEvent("Dash", baseInputsResource.dashInputEvent);
        InputMap.ActionAddEvent("Dash", loadedResource.dashInputEvent);

    }

    /// <summary>
    /// if there already exists an unwanted Savegame at the ResourcePath this function deletes that Savegame and creates a brand new one.
    /// </summary>
    private void ResetSavedResource()
    {
        LoadBaseInputEventsOnStart();
        SaveResource();
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
        if (IsKeyUsedAnywhere(replacementInput.AsText()))
        {
            GD.PrintErr("Action " + replacementInput.AsText() + " is already assigned for another action");
            return;
        }

        switch (inputType)
        {
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

    public bool IsKeyUsedAnywhere(string keyAsText)    // Checks all currently used buttons to prevent one Button being assigned to two different actions, ie left + right
    {
        keyAsText = FirstWordOfString(keyAsText);

        foreach (var element in upInputEventArray)
        {
            if (FirstWordOfString(element.AsText()) == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in downInputEventArray)
        {
            if (FirstWordOfString(element.AsText()) == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in leftInputEventArray)
        {
            if (FirstWordOfString(element.AsText()) == keyAsText)
            {
                return true;
            }
        }
        foreach (var element in rightInputEventArray)
        {
            if (FirstWordOfString(element.AsText()) == keyAsText)
            {
                return true;
            }
        }

        if (FirstWordOfString(dashInputEvent.AsText()) == keyAsText)
        {
            return true;
        }


        // Else this key is not currently used, so it can be assigned to the new action
        return false;
    }

    internal string GetTextForEvent(RemapedInputs Type, int indexInArray)
    {
        switch (Type)
        {
            case RemapedInputs.up:
                {

                    return FirstWordOfString(upInputEventArray[indexInArray].AsText());
                }

            case RemapedInputs.down:
                {
                    return FirstWordOfString(downInputEventArray[indexInArray].AsText());
                }

            case RemapedInputs.left:
                {
                    return FirstWordOfString(leftInputEventArray[indexInArray].AsText());

                }

            case RemapedInputs.right:
                {
                    return FirstWordOfString(rightInputEventArray[indexInArray].AsText());

                }

            case RemapedInputs.dash:
                {
                    return FirstWordOfString(dashInputEvent.AsText());
                }

        }
        return "Loading failed";
    }

    /// <summary>
    /// Returns the first word of a given text.
    /// </summary>
    private string FirstWordOfString(string text)       // taken from https://www.inforbiro.com/blog/c-get-first-word
    {
        string firstWord = String.Empty;

        // Check for empty string.
        if (String.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        // Get first word from passed string
        firstWord = text.Split(' ').FirstOrDefault();
        if (String.IsNullOrEmpty(firstWord))
        {
            return string.Empty;
        }

        return firstWord;
    }
}
