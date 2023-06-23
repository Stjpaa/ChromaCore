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


    static public InputsResource LoadInputsResource()
    {
        InputsResource loadedResource = new InputsResource();
        loadedResource.LoadBaseInputEventsOnStart();
        loadedResource.SaveResource();

        loadedResource = (InputsResource)ResourceLoader.Load(pathToInputsResource);

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

    public void SaveResource()
    {
        ResourceSaver.Save(this, pathToInputsResource);
    }

}
