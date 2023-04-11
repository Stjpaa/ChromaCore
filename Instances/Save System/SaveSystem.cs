using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

public static class SaveSystem
{
    private const string pathOfLevelInstantiater = "res://Instances/Save System/LevelInstantiater.tscn";

    private const string pathToLevelSelectScreen = "res://Instances/Save System/LevelSelectScene.tscn";
    private const string savePathBegining = "res://SaveData/";
    private const string savePathEnd = "SaveData.tscn";

    //public static void SaveLevel(LevelInstantiater currentlyPlayedLevel)
    //{
    //    string fileNameAndLocation = GetSaveGamePathOfScene(ResourceLoader.Load<PackedScene>(LevelInstantiater.levelToBeInstantiatedPath));

    //    PackedScene sceneToBeSaved = ScenePacker.CreatePackage(currentlyPlayedLevel.levelRoot.GetChild(0)); // we need to use GetChild(0) instead of LevelRoot, because otherwise on every save and load we would add a new Root to the scene (because the root gets added to the savefile)

    //    ResourceSaver.Save(sceneToBeSaved, fileNameAndLocation);

    //    GD.Print("Saved a file to the Path: " + fileNameAndLocation);
    //    BackToLevelSelectScreen(currentlyPlayedLevel.GetTree());
    //}

    public static void BackToLevelSelectScreen(SceneTree currentScene)
    {
        currentScene.ChangeSceneToFile(pathToLevelSelectScreen);
    }

    public static void LoadLevelWithLevelInstantiator(Level levelToBeLoaded)
    {
        if(levelToBeLoaded == null)
        {
            GD.PrintErr("no Level was assigned to give to LoadLevelWithLevelInstantiator");
            return;
        }

        if(levelToBeLoaded.baseLevelToLoad == null)
        {
            GD.PrintErr("No PackedScene was assigned to the Level which was tried to be loaded");
            return;
        }

        levelToBeLoaded.GetTree().ChangeSceneToFile(pathOfLevelInstantiater);

        LevelInstantiater.levelToBeInstantiatedPath = levelToBeLoaded.baseLevelToLoad.ResourcePath;
    }


    //public static void LoadLevel(Level levelToBeLoaded)
    //{      
    //    SceneTree currentSceneTree = levelToBeLoaded.GetTree(); // We need the current SceneTree to replace it with the loaded Level

    //    if (DoesFileExistAtPath(GetSaveGamePathOfScene(levelToBeLoaded.baseLevelToLoad)))
    //    {
    //        GD.Print("Load Savegame");
    //        currentSceneTree.ChangeSceneToFile(GetSaveGamePathOfScene(levelToBeLoaded.baseLevelToLoad));
    //    }
    //    else
    //    {
    //        GD.Print("Load Base Scene");
    //        currentSceneTree.ChangeSceneToPacked(levelToBeLoaded.baseLevelToLoad);
    //    }
    //}

    /// <summary>
    /// The baseLevel is the untouched base scene which will not be changed in any way.
    /// Instead we create a separate savegame for each level upon saving said Level.
    /// This function returns if this savegame already exists. This decides, for example, which scene needs to be loaded.
    /// </summary>
    public static bool DoesFileExistAtPath(string pathToCheck)
    {
        if (pathToCheck == null)
        {
            GD.PrintErr("path given to SaveSystem.DoesFileExistAtPath() = empty string");
            return false;
        }

        bool doesFileExist = File.Exists(ProjectSettings.GlobalizePath(pathToCheck));  // GlobalizePath is needed because File.Exists() checks from the OS Path and not the Godot path

        return doesFileExist;
    }

    //public static string GetSaveGamePathOfScene(PackedScene scene)
    //{
    //    if(scene == null)
    //    {
    //        GD.PrintErr("baseLevel given to SaveSystem.GetPathOfSavegame() does not exist");
    //        return null;
    //    }
    //    string baseLevelFileName = System.IO.Path.GetFileNameWithoutExtension(scene.ResourcePath);  // removes the path and the datatype
    //    string savegamePath = savePathBegining + baseLevelFileName + savePathEnd;

    //    return savegamePath;
    //}

    //public static void DeleteSaveGameData(PackedScene baseSceneToDeleteSaveData)
    //{
    //    if (baseSceneToDeleteSaveData == null)
    //    {
    //        GD.PrintErr("baseSceneToDeleteSaveData given to SaveSystem.DeleteSaveGameData() does not exist");
    //        return;
    //    }

    //    string PathToScene = GetSaveGamePathOfScene(baseSceneToDeleteSaveData);

    //    if (DoesFileExistAtPath(PathToScene) == true)
    //    {
    //        File.Delete(ProjectSettings.GlobalizePath(PathToScene));
    //    }
    //}

}
