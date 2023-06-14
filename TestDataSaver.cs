using Godot;
using System;

public partial class TestDataSaver : Node2D
{
    private const string pathToInputsResource = "user://InputsResource.tres";
    private InputsResource inputsResource;


    public override void _Ready()
    {
        inputsResource = new InputsResource();

        SetInputEvent();


        inputsResource.testInt = 10;
        GD.Print(inputsResource.testInt);
        SaveResource();

        inputsResource = LoadResource();
        GD.Print(inputsResource.testInt);
        GD.Print(inputsResource.inputEventTest.AsText());

    }

    private void SetInputEvent()
    {
        inputsResource.inputEventTest = InputMap.ActionGetEvents("ui_up")[1];

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
