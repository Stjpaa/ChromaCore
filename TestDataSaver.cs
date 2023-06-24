using Godot;
using System;
using System.Linq;

public partial class TestDataSaver : Node2D
{
    
    private InputsResource inputsResource;


    public override void _Ready()
    {
        inputsResource = new InputsResource();


        PrintInputEventArray();

    }


    
    private void PrintInputEventArray()
    {

        foreach (var element in inputsResource.leftInputEventArray)
        {
            GD.Print(element.AsText());
        }
    }

  


}
