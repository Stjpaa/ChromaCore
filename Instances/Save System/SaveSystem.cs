using Godot;
using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text.Json;


// saves and loads all the information of the game levels into a .json file.
// when loading the levels get their information back.
public partial class SaveSystem : Node2D
{
    private const string saveGamePath = "saveGame.json";    

    private LevelSaveData[] LevelArray;


    public override void _Ready()
    {
        LoadGameData();

        // maybe update displayed information
    }

    //private void TestSave()
    //{
    //    string jsonString = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
    //    File.WriteAllText(saveGamePath, jsonString);
    //}

    private void SaveGameData()
    {
        
        string newSavegameString ="[" ; // used to declare arrays in json to iterate over

        foreach (var level in LevelArray)
        {
            newSavegameString += level.LevelDataToJson();
            // save to json
        }

        newSavegameString += "]";

        File.WriteAllText(saveGamePath, newSavegameString);
    }

    private void LoadGameData()
    {
        if (File.Exists(saveGamePath))
        {
            GD.Print("Save game file found! it says: \n");
            GD.Print(File.ReadAllText(saveGamePath));
        }
        else
        {
            // The file doesn't exist, do something else
            GD.Print("Save game file not found!");
        }

        // save all the levels back into the array
    }



}
