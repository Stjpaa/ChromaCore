using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class LoadTestSkript : Node2D
{
    private string pathInGodot = "res://SaveData/test.json";
    private string pathGlobal;
    public override void _Ready()
    {
        pathGlobal = ProjectSettings.GlobalizePath(pathInGodot);

        GD.Print("exists");
        SaveLevelVariablesToJson();
        LoadLevelVariablesToJson();
    }

    private void SaveLevelVariablesToJson()
    {
        LevelVariablesSaveData data = new LevelVariablesSaveData();
        data.levelTimerInSeconds = 123.45;
        data.levelCompleted = true;

        // Serialize the object to a JSON string
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json_str = JsonSerializer.Serialize(data, options);

        // Write the JSON string to file
        File.WriteAllText(pathGlobal, json_str);
    }

    private void LoadLevelVariablesToJson()
    {
        if (SaveSystem.DoesFileExistAtPath(pathGlobal))
        {
            string text = File.ReadAllText(pathGlobal);

            LevelVariablesSaveData data = JsonSerializer.Deserialize<LevelVariablesSaveData>(text);

            GD.Print(data.levelTimerInSeconds);
        }
    }
}
