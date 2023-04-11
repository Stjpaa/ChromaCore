using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Reflection;


public partial class LevelInstantiater : Node2D
{
    private string pathToJson = "res://testData.json";

    public static string levelToBeInstantiatedPath;      // the Base Scene of what Level should be Loaded, we Load the SaveGame of this Scene if a Savegame exsists.
    [Export] public Node2D levelRoot;

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
        LoadLevel();
    }

    //public void SaveLevel()
    //{
    //    //SaveLevelData(levelRoot.GetChild(0));   // levelRoot does not need to be checked, because it does not belong to the level
    //    //SaveSystem.SaveLevel(this);
    //}

    private void LoadLevel()
    {
        if (SaveSystem.DoesFileExistAtPath(levelToBeInstantiatedPath) == false)
        {
            GD.Print("levelToBeInstantiatedPath in LevelInstantiater has no valid value assigned to it");
            return;
        }

        PackedScene sceneToLoad = ResourceLoader.Load<PackedScene>(levelToBeInstantiatedPath);


                    
        //string saveGamePath = SaveSystem.GetSaveGamePathOfScene(sceneToLoad);


        //if (SaveSystem.DoesFileExistAtPath(saveGamePath)) // if SaveGame already exist Load that and all of the saved values
        //{
        //    // Load Savegame and place it under node
        //    sceneToLoad = ResourceLoader.Load<PackedScene>(saveGamePath);
        //    Node TreeToAddUnderLevelRoot = sceneToLoad.Instantiate();   // we dont add this instantly to the scene to first Load every saved varibles


        //    LoadJsonSaveGame(TreeToAddUnderLevelRoot);

        //    levelRoot.AddChild(TreeToAddUnderLevelRoot);

        //}
        //else // Load base level. No extra Loading needed because references are preserved this way
        //{
        // Load BaseScene and place it under node

        //sceneToLoad = ResourceLoader.Load<PackedScene>(levelToBeInstantiatedPath);
            levelRoot.AddChild(sceneToLoad.Instantiate());
        //}
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
