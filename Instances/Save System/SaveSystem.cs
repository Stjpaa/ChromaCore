using Godot;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

public static class SaveSystem
{
    private const string pathToLevelSelectScreen = "res://Instances/Save System/LevelSelectScene.tscn";
    private const string savePathBegining = "res://SaveData/";
    private const string savePathEnd = "SaveData.tscn";

    public static void SaveLevel(Level currentlyPlayedLevel)
    {
        GD.PrintErr("change Savepath to user:// remove fixed SavePath and automatic back to main menu");

        string fileNameAndLocation = "res://SaveData/Level-PrototypeSaveData.tscn";


        PackedScene sceneToBeSaved = ScenePacker.CreatePackage(currentlyPlayedLevel.nodeWhichToPack);

        //string fileNameAndLocation = GetPathOfSavegame(currentlyPlayedLevel.baseLevel);

        ResourceSaver.Save(sceneToBeSaved, fileNameAndLocation);

        GD.Print("Saved a file to the Path: " + fileNameAndLocation);
        currentlyPlayedLevel.GetTree().ChangeSceneToFile(pathToLevelSelectScreen);
    }

    public static void LoadLevel(Level levelToBeLoaded)
    {
        
        SceneTree currentSceneTree = levelToBeLoaded.GetTree(); // We need the current SceneTree to replace it with the loaded Level

        if (DoesSavegameExistAtPath(GetSaveGamePathOfScene(levelToBeLoaded.baseLevel)))
        {
            GD.Print("Load Savegame");
            currentSceneTree.ChangeSceneToFile(GetSaveGamePathOfScene(levelToBeLoaded.baseLevel));
        }
        else
        {
            GD.Print("Load Base Scene");
            currentSceneTree.ChangeSceneToPacked(levelToBeLoaded.baseLevel);
        }
    }

    /// <summary>
    /// The baseLevel is the untouched base scene which will not be changed in any way.
    /// Instead we create a separate savegame for each level upon saving said Level.
    /// This function returns if this savegame already exists. This decides, for example, which scene needs to be loaded.
    /// </summary>
    private static bool DoesSavegameExistAtPath(string pathToCheck)
    {
        if(pathToCheck == null)
        {
            GD.PrintErr("path given to SaveSystem.DoesSavegameExistFor() = empty string");
            return false;
        }

        bool doesFileExist = File.Exists(ProjectSettings.GlobalizePath(pathToCheck));  // GlobalizePath is needed because File.Exists() checks from the OS Path and not the Godot path

        return doesFileExist;
    }

    private static string GetSaveGamePathOfScene(PackedScene scene)
    {
        if(scene == null)
        {
            GD.PrintErr("baseLevel given to SaveSystem.GetPathOfSavegame() does not exist");
            return null;
        }
        string baseLevelFileName = System.IO.Path.GetFileNameWithoutExtension(scene.ResourcePath);  // removes the path and the datatype
        string savegamePath = savePathBegining + baseLevelFileName + savePathEnd;

        return savegamePath;
    }

    public static void DeleteSaveGameData(PackedScene baseSceneToDeleteSaveData)
    {
        if (baseSceneToDeleteSaveData == null)
        {
            GD.PrintErr("baseSceneToDeleteSaveData given to SaveSystem.DeleteSaveGameData() does not exist");
            return;
        }

        string PathToScene = GetSaveGamePathOfScene(baseSceneToDeleteSaveData);

        if (DoesSavegameExistAtPath(PathToScene) == true)
        {
            File.Delete(ProjectSettings.GlobalizePath(PathToScene));
        }

    }

}
