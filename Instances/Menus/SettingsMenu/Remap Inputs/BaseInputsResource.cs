using Godot;
using System;

/// <summary>
/// saves the base Values of the InputMap at start of game, so that you can always compare the current inputs to the values at start. 
/// this is needed to reassign the loaded values to the inputmap on start of game
/// </summary>
public partial class BaseInputsResource : Resource
{
    const string pathToBaseInputsResource = "user://BaseInputsResource.tres";

    [Export] public InputEvent[] upInputEventArray = { null, null };    
    [Export] public InputEvent[] downInputEventArray = { null, null };
    [Export] public InputEvent[] leftInputEventArray = { null, null };
    [Export] public InputEvent[] rightInputEventArray = { null, null };
    [Export] public InputEvent dashInputEvent = null;

    public static BaseInputsResource LoadBaseInputResource()    // Loads the Base Resource or creates it if it doesnt exist yet.
    {
        BaseInputsResource loadedBaseResource;
        if (SaveSystem.DoesFileExistAtPath(pathToBaseInputsResource))
        {
            loadedBaseResource = (BaseInputsResource)ResourceLoader.Load(pathToBaseInputsResource);
        }
        else      // if no Resource exists instead Load the Base
        {
            loadedBaseResource = new BaseInputsResource();
            loadedBaseResource.AssignValuesAtStart();
            ResourceSaver.Save(loadedBaseResource, pathToBaseInputsResource);
        }

        return loadedBaseResource;
    }




    private void AssignValuesAtStart()  // assignes the values to correspond to the inputMap Values.
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



}
