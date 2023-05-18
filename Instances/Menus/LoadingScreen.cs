using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public partial class LoadingScreen : Control
{
    private AnimationPlayer loadingscreenAnimationPlayer;
    private Panel loadingScreenBackground;
    [Export]private TextureRect startPlanet;
    [Export]private TextureRect destinationPlanet;


    [Export] private PackedScene testLevelToLoad;



    [Export] private Texture2D testPlanetOne;
    [Export] private Texture2D testPlanetTwo;

    //[Export]

    public override void _Ready()
    {
        loadingScreenBackground = (Panel)GetNode("LoadingScreenBackground");
        loadingscreenAnimationPlayer = (AnimationPlayer)GetNode("LoadingScreenAP");

        HideLoadingScreen();


        StartLoadSceneAsync(testLevelToLoad.ResourcePath);

    }

    private void StartLoadSceneAsync(string sceneToLoadPath)
    {
        //_ = LoadSceneAsync(sceneToLoadPath);    // _ = means the value gets discarded on completion 

        _ = LoadingScreenAsync(testPlanetOne, testPlanetTwo);    // _ = means the value gets discarded on completion 
    }

    //private async Task LoadSceneAsync(string sceneToLoad)
    //{
    //    ResourceLoader.LoadThreadedRequest(sceneToLoad);    // initiate the Background Loading

    //    var sceneLoadStatus = ResourceLoader.LoadThreadedGetStatus(sceneToLoad);





    //    do    // ensures the middle animation always gets played at least once
    //    {
    //        await LoadingScreenLoopAnimation();
    //    } while (sceneLoadStatus == ResourceLoader.ThreadLoadStatus.InProgress);


    //    // maybe switch scene and pause game before fading out the loading screen
    //    await LoadingScreenEndAnimation();

    //    var loadedScene = (PackedScene)ResourceLoader.LoadThreadedGet(sceneToLoad);     // Change to the Loaded Scene
    //    GetTree().ChangeSceneToPacked(loadedScene);


    //}


    public async Task LoadingScreenAsync(Texture2D startLocation, Texture2D destination)
    {
        ShowLoadingScreen();
        startPlanet.Texture = startLocation;
        destinationPlanet.Texture = destination;    

        loadingscreenAnimationPlayer.Play("RocketFlight");

        while (loadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
        {
            // Wait for a short time before checking again
            await Task.Delay(50);
        }


        HideLoadingScreen();

        // done
    }

    private void HideLoadingScreen()
    {
        loadingScreenBackground.Visible = false;
        loadingScreenBackground.ProcessMode = ProcessModeEnum.Disabled;
    }

    private void ShowLoadingScreen()
    {
        loadingScreenBackground.Visible = true;
        loadingScreenBackground.ProcessMode = ProcessModeEnum.Inherit;
    }


    //private async Task LoadingScreenStartAnimation()
    //{
    //    LoadingscreenAnimationPlayer.Play("LoadingScreenStartAnimation");

    //    while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
    //    {
    //        // Wait for a short time before checking again
    //        await Task.Delay(50);
    //    }
    //}

    //async Task LoadingScreenEndAnimation()
    //{
    //    LoadingscreenAnimationPlayer.Play("LoadingScreenEndAnimation");

    //    while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
    //    {
    //        // Wait for a short time before checking again
    //        await Task.Delay(50);
    //    }
    //}

    //async Task LoadingScreenLoopAnimation()
    //{
    //    LoadingscreenAnimationPlayer.Play("LoadingScreenLoopAnimation");

    //    while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
    //    {
    //        // Wait for a short time before checking again
    //        await Task.Delay(50);
    //    }
    //}
}
