using Godot;
using System;

public partial class Level : Control
{
    [Export] public PackedScene baseLevelToLoad;
    //[Export] public Node2D nodeWhichToPack;
    private LevelVariablesSaveData levelVariablesSaveData;

    private LevelSelectVisualisation dataVisualisation;
    private TextureRect planetVisualisation;

    [Export] public Texture2D planetTexture;


    public override void _Ready()
    {
        dataVisualisation = (LevelSelectVisualisation)GetNode("LevelSelectVisualisation");
        planetVisualisation = (TextureRect)GetNode("PlanetVisualisation");
        UpdateValues();
    }

    private void UpdateValues()
    {
        LoadSaveData();

        DisplayLevelTime();

        DisplayPlanetTexture();
    }

    public void LoadSaveData()
    {
        if (baseLevelToLoad == null)
        {
            levelVariablesSaveData = new LevelVariablesSaveData();
            return;
        }

        levelVariablesSaveData = SaveSystem.LoadLevelVariablesSaveData(baseLevelToLoad);


        if (planetTexture != null)
        {

            if (levelVariablesSaveData.planetTexturePath != planetTexture.ResourcePath)       // set The Planet Texture To be referenced for the Loading Screen
            {

                levelVariablesSaveData.planetTexturePath = planetTexture.ResourcePath;
                SaveSystem.SaveLevelVariablesToJson(baseLevelToLoad, levelVariablesSaveData);
            }
        }
    }


    public void LoadLevel()
    {
        SaveSystem.LoadLevelWithLevelInstantiator(this);

    }



    private void DisplayLevelTime()
    {
        dataVisualisation.VisualizeData(levelVariablesSaveData);
    }

    private void DisplayPlanetTexture()
    {
        planetVisualisation.Texture = planetTexture;
    }

    public void DeleteSaveData()
    {
        SaveSystem.DeleteLevelVariableSaveData(baseLevelToLoad);

        UpdateValues();
    }
}
