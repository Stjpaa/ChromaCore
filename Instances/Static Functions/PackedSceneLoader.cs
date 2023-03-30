using Godot;
using System;

public static class PackedSceneLoader
{
    public static void LoadScene(string packedScenePath, SceneTree currentSceneTree)
    {
        // Load the PackedScene from savepath
        var packedSceneToBeLoaded = (PackedScene)ResourceLoader.Load(packedScenePath);

        // Replace the current scene with the loaded scene
        currentSceneTree.ChangeSceneToPacked(packedSceneToBeLoaded);
    }
}
