using Godot;
using PlayerController;
using System;
using System.Net.Http;
using System.Text;

public partial class ServerCommunicationManager : Node2D
{
    [Export] private int level = 1;
    [Export] private PlayerController2D player; // needed for his position on death/ RQ
    [Export] private LevelTimer levelTimer;
    [Export] private win_condition win_Condition;

    private const string deathUrl = "https://chroma-core.franek-stratovarius.duckdns.org/death";
    private const string winUrl = "https://chroma-core.franek-stratovarius.duckdns.org/win";
    private const string rQUrl = "https://chroma-core.franek-stratovarius.duckdns.org/ragequit";

    public override void _Ready()
    {
        if (levelTimer == null)
        {
            GD.PrintErr("Timer not assigned in ServerCommunicationManager");
        }

        if (player == null)
        {
            GD.PrintErr("Player not assigned in ServerCommunicationManager");
        }
        else
        {
            var callable = new Callable(this, "DeathSignal");
            player.Connect(nameof(player.playerDeath), callable);
        }

        if (win_Condition != null)
        {
            var callable = new Callable(this, "WinSignal");
            win_Condition.Connect(nameof(win_Condition.winGame), callable);
        }
        else
        {
            GD.PrintErr("wincondition not assigned in ServerCommunicationManager");
        }



    }

    private void DeathSignal(float deathPosX, float deathPosY)
    {
        float playTime = (float)levelTimer.timeLevelWasPlayedInSeconds;
        SendDeathOrRQ(deathUrl, playTime, deathPosX, deathPosY);
    }

    public void RQSignal(float posX, float posY)
    {
        float playTime = (float)levelTimer.timeLevelWasPlayedInSeconds;
        SendDeathOrRQ(rQUrl, playTime, posX, posY);
    }

    public void WinSignal()
    {
        GD.Print("WinSignalHappended");
        float playTime = (float)levelTimer.timeLevelWasPlayedInSeconds;
        var dataAsDict = new Godot.Collections.Dictionary
        {
            { "time", playTime},
            { "level", level }
        };

        string message = Json.Stringify(dataAsDict);

        var httpRequest = new HttpRequest();
        AddChild(httpRequest);

        Error error = httpRequest.Request(winUrl, null, Godot.HttpClient.Method.Post, message);
        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }
    private void SendDeathOrRQ(string url, float time, float posX, float posY)
    {

        var dataAsDict = new Godot.Collections.Dictionary
        {
            { "time", time },
            { "level", level },
            { "positionx", posX },
            { "positiony", posY }
        };

        string message = Json.Stringify(dataAsDict);

        var httpRequest = new HttpRequest();
        AddChild(httpRequest);


        Error error = httpRequest.Request(url, null, Godot.HttpClient.Method.Post, message);
        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }

}
