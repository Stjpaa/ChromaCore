using Godot;
using System;
using System.Collections;

public partial class LoadingScreen : Control
{
    //private PackedScene loadingScene;

    //public override void _Ready()
    //{
    //    loadingScene = GD.Load<PackedScene>("res://Load.tscn");
    //}

    //public void LoadScene(Node currentScene, string nextScene)
    //{
    //    // Add loading scene to the root
    //    var loadingSceneInstance = (Node)loadingScene.Instantiate();
    //    GetTree().Root.CallDeferred("AddChild", loadingSceneInstance);

    //    // Find the targeted scene
    //    var loader = ResourceLoader.LoadInteractive(nextScene);
        

    //    // Check for errors
    //    if (loader == null)
    //    {
    //        // Handle your error
    //        GD.Print("Error occurred while getting the scene");
    //        return;
    //    }

    //    currentScene.QueueFree();
    //    // Creating a little delay, that lets the loading screen to appear.
    //    IEnumerator CreateTimer()
    //    {
    //        yield return GetTree().CreateTimer(0.5f);
    //    }
    //    StartCoroutine(CreateTimer());

    //    // Loading the next_scene using Poll() function
    //    // Since Poll() function loads data in chunks thus we need to put that in loop
    //    while (true)
    //    {
    //        var error = loader.Poll();
    //        // When we get a chunk of data
    //        if (error == Error.Ok)
    //        {
    //            // Update the progress bar according to amount of data loaded
    //            var progressBar = (ProgressBar)loadingSceneInstance.GetNode("ProgressBar");
    //            progressBar.Value = (float)loader.GetStage() / loader.GetStageCount() * 100f;
    //        }
    //        // When all the data have been loaded
    //        else if (error == Error.FileEof)
    //        {
    //            // Creating scene instance from loaded data
    //            var scene = (Node)loader.GetResource().Instance();
    //            // Adding scene to the root
    //            GetTree().Root.CallDeferred("AddChild", scene);
    //            // Removing loading scene
    //            loadingSceneInstance.QueueFree();
    //            return;
    //        }
    //        else
    //        {
    //            // Handle your error
    //            GD.Print("Error occurred while loading chunks of data");
    //            return;
    //        }
    //    }
    //}
}
