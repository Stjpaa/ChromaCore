using Godot;
using System;
using System.Linq;

public partial class TestDataSaver : Node2D
{
    private const string pathToInputsResource = "user://InputsResource.tres";
    private InputsResource inputsResource;


    public override void _Ready()
    {
        inputsResource = new InputsResource();

        SetInputEventArray();
        GD.Print("Array " + inputsResource.downInputEventArray[0].AsText());

        SaveResource();

        inputsResource = LoadResource();


        GD.Print("Array Loaded " + inputsResource.downInputEventArray[0].AsText());
        //GD.Print("Array " + inputsResource.inputEventArray[1].AsText());

    }


    
    private void SetInputEventArray()
    {

        inputsResource.downInputEventArray[0] = InputMap.ActionGetEvents("ui_up")[0];
        //inputsResource.inputEventArray.Append(InputMap.ActionGetEvents("ui_up")[1]);
    }

    private InputsResource LoadResource()
    {
        InputsResource loadedResource = (InputsResource)ResourceLoader.Load(pathToInputsResource);

        if (loadedResource != null)
        {
            GD.Print("succesfully loaded");
            return loadedResource;
        }
        else
        {
            return new InputsResource(); 
        }
        
    }

    private void SaveResource()
    {
        ResourceSaver.Save(inputsResource, pathToInputsResource);
    }
}
