using Godot;
using System;

public partial class LevelInstantiater : Node2D
{
    public static string levelToBeInstantiatedPath;      // the Base Scene of what Level should be Loaded, we Load the SaveGame of this Scene if a Savegame exsists.
    [Export] public Node2D levelRoot;

    public override void _Ready()
    {
        if (!SaveSystem.DoesFileExistAtPath(levelToBeInstantiatedPath))
        {
            GD.PrintErr("no Level exsists at levelToBeInstantiatedPath in LevelInstantiater");
            return;
        }

        if (levelRoot == null)
        {
            GD.PrintErr("levelRoot was not assigned in the LevelInstantiater Scene");
            return;
        }
        LoadLevel();
    }

    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    }

    private void LoadLevel()
    {
        if (SaveSystem.DoesFileExistAtPath(levelToBeInstantiatedPath) == false)
        {
            GD.Print("levelToBeInstantiatedPath in LevelInstantiater has no valid value assigned to it");
            return;
        }

        PackedScene sceneToLoad = ResourceLoader.Load<PackedScene>(levelToBeInstantiatedPath);

        string saveGamePath = SaveSystem.GetSaveGamePathOfScene(sceneToLoad);

        
        if (SaveSystem.DoesFileExistAtPath(saveGamePath)) // if SaveGame already exist Load that and all of the saved values
        {
            // Load Savegame and place it under node
            sceneToLoad = ResourceLoader.Load<PackedScene>(saveGamePath);
            levelRoot.AddChild(sceneToLoad.Instantiate());
        }
        else // Load base level. No extra Loading needed because references are preserved this way
        {
            // Load BaseScene and place it under node

            sceneToLoad = ResourceLoader.Load<PackedScene>(levelToBeInstantiatedPath);
            levelRoot.AddChild(sceneToLoad.Instantiate());
        }

        
    }

}
