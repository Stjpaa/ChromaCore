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

        SetInputEvent();
        SetInputEventArray();
        GD.Print("Array " + inputsResource.inputEventArray[0].AsText());

        inputsResource.intArray[0] = -5;
        inputsResource.intArray[1] = 11;
        inputsResource.testInt = 10;
        GD.Print(inputsResource.testInt);
        SaveResource();

        inputsResource = LoadResource();
        GD.Print(inputsResource.testInt);
        GD.Print(inputsResource.testInputEvent.AsText());
        GD.Print("Array " + inputsResource.intArray[0]);
        GD.Print("Array " + inputsResource.intArray[1]);
        GD.Print("Array " + inputsResource.inputEventArray[0].AsText());
        //GD.Print("Array " + inputsResource.inputEventArray[1].AsText());

    }

    private void SetInputEvent()
    {
        inputsResource.testInputEvent = InputMap.ActionGetEvents("ui_up")[1];
    }
    
    private void SetInputEventArray()
    {

        inputsResource.inputEventArray[0] = InputMap.ActionGetEvents("ui_up")[0];
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
