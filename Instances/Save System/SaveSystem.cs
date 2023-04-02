using Godot;
using System;
using System.Runtime.CompilerServices;

public static class SaveSystem
{

    public const string savePath = "res://SaveData/";

    public static void SaveLevel(Level currentlyPlayedLevel)
    {
        PackedScene sceneToBeSaved = ScenePacker.CreatePackage(currentlyPlayedLevel);
        
        string fileNameAndLocation = GetPathOfSavegame(currentlyPlayedLevel.baseLevel);

        ResourceSaver.Save(sceneToBeSaved, fileNameAndLocation);

        GD.Print("Saved a file to the Path: " + fileNameAndLocation);
    }

    public static void LoadLevel(Level levelToBeLoaded)
    {
        SceneTree currentSceneTree = levelToBeLoaded.GetTree(); // We need the current SceneTree to replace it with the loaded Level

        if (DoesSavegameExistFor(levelToBeLoaded.baseLevel))
        {
            currentSceneTree.ChangeSceneToFile(GetPathOfSavegame(levelToBeLoaded.baseLevel));
        }
        else
        {
            currentSceneTree.ChangeSceneToPacked(levelToBeLoaded.baseLevel);
        }
    }

    /// <summary>
    /// The baseLevel is the untouched base scene which will not be changed in any way.
    /// Instead we create a separate savegame for each level upon saving said Level.
    /// This function returns if this savegame already exists. This decides, for example, which scene needs to be loaded.
    /// </summary>
    public static bool DoesSavegameExistFor(PackedScene baseLevel)
    {
        if(baseLevel == null)
        {
            GD.PrintErr("baseLevel given to SaveSystem.DoesSavegameExistFor() does not exist");
            return false;
        }

        string savegameLocation = GetPathOfSavegame(baseLevel);

        return System.IO.File.Exists(savegameLocation);
    }

    private static string GetPathOfSavegame(PackedScene baseLevel)
    {
        if(baseLevel == null)
        {
            GD.PrintErr("baseLevel given to SaveSystem.GetPathOfSavegame() does not exist");
            return null;
        }
        string baseLevelFileName = System.IO.Path.GetFileNameWithoutExtension(baseLevel.ResourcePath);  // removes the path and the datatype
        string savegamePath = SaveSystem.savePath + baseLevelFileName + "SaveData.tscn";

        return savegamePath;
    }

}
