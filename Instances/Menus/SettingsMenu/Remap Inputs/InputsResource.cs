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

        for (int i = 0; i <= 1; i++)
        {
            loadedResource.DeleteExistingActions("ui_up", baseInputsResource.upInputEventArray[i]);
            loadedResource.DeleteExistingActions("Jump", baseInputsResource.upInputEventArray[i]);

            InputMap.ActionAddEvent("ui_up", loadedResource.upInputEventArray[i]);
            InputMap.ActionAddEvent("Jump", loadedResource.upInputEventArray[i]);


            loadedResource.DeleteExistingActions("ui_down", baseInputsResource.downInputEventArray[i]);
            InputMap.ActionAddEvent("ui_down", loadedResource.downInputEventArray[i]);


            loadedResource.DeleteExistingActions("ui_left", baseInputsResource.leftInputEventArray[i]);
            loadedResource.DeleteExistingActions("Move_Left", baseInputsResource.leftInputEventArray[i]);

            InputMap.ActionAddEvent("ui_left", loadedResource.leftInputEventArray[i]);
            InputMap.ActionAddEvent("Move_Left", loadedResource.leftInputEventArray[i]);


            loadedResource.DeleteExistingActions("ui_right", baseInputsResource.rightInputEventArray[i]);
            loadedResource.DeleteExistingActions("Move_Right", baseInputsResource.rightInputEventArray[i]);

            InputMap.ActionAddEvent("ui_right", loadedResource.rightInputEventArray[i]);
            InputMap.ActionAddEvent("Move_Right", loadedResource.rightInputEventArray[i]);

        }

        loadedResource.DeleteExistingActions("Dash", baseInputsResource.dashInputEvent);
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
                    DeleteExistingActions("ui_up", upInputEventArray[indexOfButton]);
                    DeleteExistingActions("Jump", upInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_up", replacementInput);
                    InputMap.ActionAddEvent("Jump", replacementInput);

                    upInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change

                }
                break;

            case RemapedInputs.down:
                {
                    DeleteExistingActions("ui_down", downInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_down", replacementInput);

                    downInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change
                }
                break;

            case RemapedInputs.left:
                {
                    DeleteExistingActions("ui_left", leftInputEventArray[indexOfButton]);
                    DeleteExistingActions("Move_Left", leftInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_left", replacementInput);
                    InputMap.ActionAddEvent("Move_Left", replacementInput);

                    leftInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change

                }
                break;

            case RemapedInputs.right:
                {
                    DeleteExistingActions("ui_right", rightInputEventArray[indexOfButton]);
                        DeleteExistingActions("Move_Right", rightInputEventArray[indexOfButton]);

                    InputMap.ActionAddEvent("ui_right", replacementInput);
                    InputMap.ActionAddEvent("Move_Right", replacementInput);

                    rightInputEventArray[indexOfButton] = replacementInput;
                    SaveResource(); // Save The change

                }
                break;

            case RemapedInputs.dash:
                {
                    DeleteExistingActions("Dash", dashInputEvent);
                    InputMap.ActionAddEvent("Dash", replacementInput);

                    dashInputEvent = replacementInput;
                    SaveResource();
                }
                break;
        }

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

    /// <summary>
    /// needed because the saved InputEvent will not always be exactly the same as the one on the InputMap. this ensures, that the correct Action gets deleted.
    /// </summary>
    /// <param name="inputAction"></param>
    /// <param name="eventToCheckFor"></param>
    private void DeleteExistingActions(string inputAction, InputEvent eventToCheckFor)
    {
        foreach (var KeyToCheck in InputMap.ActionGetEvents(inputAction))
        {
            if (FirstWordOfString(eventToCheckFor.AsText()) == FirstWordOfString(KeyToCheck.AsText()))
            {
                InputMap.ActionEraseEvent(inputAction, KeyToCheck);
            }
        }
    }
}
