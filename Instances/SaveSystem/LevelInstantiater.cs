using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Reflection;

using System.Threading.Tasks;


public partial class LevelInstantiater : Node2D
{
    public string levelToBeInstantiatedPath;      // the Base Scene of what Level should be Loaded, we Load the SaveGame of this Scene if a Savegame exsists.
    private PackedScene sceneToLoad;    
    
    [Export] public Node2D levelRoot;
    [Export] private LoadingScreen loadingScreen;

    public LevelVariablesSaveData levelSaveData;

    private string levelVariableSaveDataGlobalPath;

    private LevelManager levelManager;      // responsible for creating the updated SaveData for the current Level


    public override void _Ready()
    {

        //if (!SaveSystem.DoesFileExistAtPath(levelToBeInstantiatedPath))
        //{
        //    GD.PrintErr("no Level exsists at levelToBeInstantiatedPath in LevelInstantiater");
        //    return;
        //}

        if (levelRoot == null)
        {
            GD.PrintErr("levelRoot was not assigned in the LevelInstantiater Scene");
            return;
        }



        _ = InstanciateLevelAsync();    

    }

    public void SetLoadingScreenTextures(Texture2D startTexture, Texture2D destinationTexture)
    {
        loadingScreen.SetPlanetTextures(startTexture, destinationTexture);
    }

    //public LevelVariablesSaveData LoadLevelVariablesSaveData(PackedScene sceneToLoad)
    //{
    //    if (SaveSystem.DoesFileExistAtPath(SaveSystem.GetLevelVariablesSaveDataPath(sceneToLoad)))
    //    {
    //        string text = File.ReadAllText(levelVariableSaveDataGlobalPath);

    //        return JsonSerializer.Deserialize<LevelVariablesSaveData>(text);

    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    

    public void SaveLevelVariables()
    {
        SaveSystem.SaveLevelVariablesToJson(sceneToLoad, levelManager.CreateUpdatedLevelVariablesSaveData());
    }


    //public void SaveLevel()
    //{
    //    //SaveLevelData(levelRoot.GetChild(0));   // levelRoot does not need to be checked, because it does not belong to the level
    //    //SaveSystem.SaveLevel(this);
    //}

    private async Task InstanciateLevelAsync()
    {
        if (SaveSystem.DoesFileExistAtPath(levelToBeInstantiatedPath) == false)
        {
            GD.Print("levelToBeInstantiatedPath in LevelInstantiater has no valid value assigned to it");
            return;
        }



        sceneToLoad = ResourceLoader.Load<PackedScene>(levelToBeInstantiatedPath);

        ResourceLoader.LoadThreadedRequest(levelToBeInstantiatedPath);    // initiate the Background Loading


        SetSaveData();

        loadingScreen.SetPlanetTextures(loadingScreen.homePlaneTexture, (Texture2D)ResourceLoader.Load(levelSaveData.planetTexturePath));

        await loadingScreen.LoadingScreenAsync();

        var loadedScene = (PackedScene)ResourceLoader.LoadThreadedGet(levelToBeInstantiatedPath);     // Change to the Loaded Scene

        levelRoot.AddChild(loadedScene.Instantiate());





        levelManager = (LevelManager)levelRoot.GetChild(0).GetNode("LevelManager");

        if (levelManager == null)
        {
            GD.PrintErr("Add LevelManager to Level");
            return;
        }

        levelManager.InstantiateValues(levelSaveData);
    }

    private void SetSaveData()
    {
        levelVariableSaveDataGlobalPath = ProjectSettings.GlobalizePath(SaveSystem.GetLevelVariablesSaveDataPath(sceneToLoad));   // we need the global path to read/write with File.ReadAllText

        levelSaveData = SaveSystem.LoadLevelVariablesSaveData(sceneToLoad);

        if (levelSaveData == null)
        {
            levelSaveData = new LevelVariablesSaveData();
        }
    }

    //private void LoadJsonSaveGame(Node rootNodeOfLoadedScene)
    //{
    //    // open json file and iterate over the file, each node that actually calls gets the next line/ object of the file
    //    CallLoadForEachChildNode(rootNodeOfLoadedScene);
    //}

    //private void CallLoadForEachChildNode(Node parentNode)
    //{
    //    string test = (string)parentNode.Call("LoadOnInstantiation");       // executes the script on each node if it exists on it



    //    // Recursively write all child nodes
    //    foreach (Node child in parentNode.GetChildren())
    //    {
    //        CallLoadForEachChildNode(child);
    //    }
    //}

    //private void SaveLevelData(Node node)
    //{
    //    node.Call("SaveOnInstantiation");       // executes the script on each node if it exists on it

    //    // Recursively write all child nodes
    //    foreach (Node child in node.GetChildren())
    //    {
    //        SaveLevelData(child);
    //    }
    //}



}
