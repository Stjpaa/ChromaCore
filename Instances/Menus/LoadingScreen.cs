using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public partial class LoadingScreen : Control
{
    private AnimationPlayer LoadingscreenAnimationPlayer;

    [Export] private PackedScene testLevelToLoad;

    public override void _Ready()
    {
        LoadingscreenAnimationPlayer = (AnimationPlayer)GetNode("LoadingScreenAnimation");
        StartLoadSceneAsync(testLevelToLoad.ResourcePath);
    }

    private void StartLoadSceneAsync(string sceneToLoadPath)
    {
        _ = LoadSceneAsync(sceneToLoadPath);    // _ = means the value gets discarded on completion 
    }

    private async Task LoadSceneAsync(string sceneToLoad)
    {
        ResourceLoader.LoadThreadedRequest(sceneToLoad);    // initiate the Background Loading

        var sceneLoadStatus = ResourceLoader.LoadThreadedGetStatus(sceneToLoad);



        await LoadingScreenStartAnimation();




        do    // ensures the middle animation always gets played at least once
        {
            await LoadingScreenLoopAnimation();
        } while (sceneLoadStatus == ResourceLoader.ThreadLoadStatus.InProgress);


        // maybe switch scene and pause game before fading out the loading screen
        await LoadingScreenEndAnimation();

        var loadedScene = (PackedScene)ResourceLoader.LoadThreadedGet(sceneToLoad);     // Change to the Loaded Scene
        GetTree().ChangeSceneToPacked(loadedScene);


    }


    private async Task LoadingScreenStartAnimation()
    {
        LoadingscreenAnimationPlayer.Play("LoadingScreenStartAnimation");

        while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
        {
            // Wait for a short time before checking again
            await Task.Delay(50);
        }
    }

    async Task LoadingScreenEndAnimation()
    {
        LoadingscreenAnimationPlayer.Play("LoadingScreenEndAnimation");

        while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
        {
            // Wait for a short time before checking again
            await Task.Delay(50);
        }
    }

    async Task LoadingScreenLoopAnimation()
    {
        LoadingscreenAnimationPlayer.Play("LoadingScreenLoopAnimation");

        while (LoadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
        {
            // Wait for a short time before checking again
            await Task.Delay(50);
        }
    }
}
