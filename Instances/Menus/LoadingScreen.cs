using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public partial class LoadingScreen : CanvasLayer
{
    private AnimationPlayer loadingscreenAnimationPlayer;
    private Panel loadingScreenBackground;
    [Export] private TextureRect startPlanet;
    [Export] private TextureRect destinationPlanet;


    [Export] public Texture2D homePlanetTexture;

    public override void _Ready()
    {
        loadingScreenBackground = (Panel)GetNode("LoadingScreenBackground");
        loadingscreenAnimationPlayer = (AnimationPlayer)GetNode("LoadingScreenAP");

        HideLoadingScreen();
    }

    public void SetPlanetTextures(Texture2D startTexture,Texture2D destinationTexture)
    {
        startPlanet.Texture = startTexture; 
        destinationPlanet.Texture = destinationTexture;
    }

    public async Task LoadingScreenAsync()
    {
        GD.Print("loadingscreen start");
        ShowLoadingScreen();

        loadingscreenAnimationPlayer.Play("RocketFlight");

        while (loadingscreenAnimationPlayer.IsPlaying())    // wait until Animation is done
        {
            // Wait for a short time before checking again
            await Task.Delay(5);
        }
        HideLoadingScreen();
        GD.Print("loadingscreen end");

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

}
