using Godot;
using System;
using System.Runtime.CompilerServices;

public static class SaveSystem
{

    public const string savePath = "res://SaveData/";

    public static void SaveScene(Node rootNodeOfSavedScene)
    {
        PackedScene sceneToBeSaved = ScenePacker.CreatePackage(rootNodeOfSavedScene);

        GD.Print("path given by Node: " + rootNodeOfSavedScene.Name);
        
        string fileNameAndLocation = savePath + rootNodeOfSavedScene.Name + ".tscn";

        ResourceSaver.Save(sceneToBeSaved, fileNameAndLocation);
    }

    public static void LoadSceneByPath(string pathOfSavedLevel, SceneTree currentSceneTree)
    {
        //currentSceneTree.ChangeSceneToPacked(LevelToBeLoaded);
        currentSceneTree.ChangeSceneToFile(pathOfSavedLevel);
    }

    public static void LoadSceneByPackedScene(PackedScene LevelToBeLoaded, SceneTree currentSceneTree)
    {
        currentSceneTree.ChangeSceneToPacked(LevelToBeLoaded);
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

        string baseLevelFileName = System.IO.Path.GetFileNameWithoutExtension(baseLevel.ResourcePath);  // removes the path and the datatype
        string savegameLocation = SaveSystem.savePath + baseLevelFileName + "SaveData.tscn";

        
        return System.IO.File.Exists(savegameLocation);
    }
}
