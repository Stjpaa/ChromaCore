using Godot;
using System;

public partial class InputsResource : Resource
{
    private const string pathToInputsResource = "user://InputsResource.tres";

    [Export] public InputEvent[] upInputEventArray = { null, null };    // Arrays have to have right starting size
    [Export] public InputEvent[] downInputEventArray = { null, null };
    [Export] public InputEvent[] leftInputEventArray = { null, null };
    [Export] public InputEvent[] rightInputEventArray = { null, null };
    [Export] public InputEvent dashInputEventArray = null;


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
        dashInputEventArray = InputMap.ActionGetEvents("Dash")[0];
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

        if(dashInputEventArray.AsText() == keyAsText)
        {
            return true;
        }


        // Else this key is not currently used, so it can be assigned to the new action
        return false;
    }
}
