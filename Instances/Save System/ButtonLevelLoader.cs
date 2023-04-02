using Godot;
using System;

public partial class ButtonLevelLoader : Button
{
    [Export] public PackedScene LevelToBeLoaded;
    [Export] public string PathLevelToBeLoaded;


    public override void _Pressed()
    {
        LoadLevelOnPress();
    }


    private void LoadLevelOnPress()
    {
        if(LevelToBeLoaded == null)
        {
            if (PathLevelToBeLoaded != null)
            {
                GD.Print("LoadSceneByPath");
                SaveSystem.LoadSceneByPath(PathLevelToBeLoaded,GetTree());
            }

            GD.PrintErr("no PackedScene assigned to: ", this.GetPath());
            return;
        }

        SaveSystem.LoadSceneByPackedScene(LevelToBeLoaded, GetTree());
    }
}
