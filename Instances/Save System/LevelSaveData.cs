using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

// all the Information about a singular Level which has to be saved by the Savesystem
// including an Array of all the unique objects within the Level.
// I.e. positions of moved objects, current state of objects (on/off), which objects were removed etc.

public partial class LevelSaveData
{
    public string levelName;

    public List<string> levelObjectsSaves;

    public string LevelDataToJson()
    {
        string levelDataInJson;

        levelDataInJson = "{\"name\":levelName,\"age\":30,\"city\":\"New York\"}";

        return levelDataInJson;
    }

    // private ObjectsInScene[] ObjectsWhichShouldBeSaved

}
