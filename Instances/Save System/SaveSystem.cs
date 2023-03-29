using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Reflection.Emit;
using System.Text.Json;


// saves and loads all the information of the game levels into a .json file.
// when loading the levels get their information back.
public partial class SaveSystem : Node2D
{
    //public class SaveData
    //{
    //    public List<string> levelsAsStrings { get; set; }


    //}

    //private const string saveGamePath = "saveGame.json";

    //private LevelSaveData[] LevelArray;


    //public override void _Ready()
    //{
    //    SaveGameData();
    //    LoadGameData();

    //    // maybe update displayed information
    //}

    //private void SaveGameData()
    //{

    //    List<string> localLevelsAsStrings = new List<string>();

    //    if (LevelArray == null)
    //    {
    //        localLevelsAsStrings.Add("LevelArray empty");
    //        GD.Print("LevelArral was not instanciated");
    //    }
    //    else
    //    {
    //        for (int i = 0; i < LevelArray.Count(); i++)
    //        {
    //            localLevelsAsStrings.Add(LevelArray[i].LevelDataToJson());
    //        }
    //    }

    //    SaveData newSaveData = new SaveData();
    //    newSaveData.levelsAsStrings = localLevelsAsStrings;
    //    // convert SaveData to Json



    //    File.WriteAllText(saveGamePath, JsonSerializer.Serialize(newSaveData));

    //    GD.Print(JsonSerializer.Serialize(newSaveData));
    //}

    //private void LoadGameData()
    //{
    //    if (File.Exists(saveGamePath))
    //    {
    //        GD.Print("Save game file found! it says: \n");
    //        GD.Print(File.ReadAllText(saveGamePath));
    //    }
    //    else
    //    {
    //        // The file doesn't exist, do something else
    //        GD.Print("Save game file not found!");
    //    }

    //    // save all the levels back into the array
    //}

    
}


