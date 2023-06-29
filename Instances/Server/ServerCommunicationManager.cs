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


    public override void _Ready()
    {
        if (levelTimer == null)
        {
            GD.PrintErr("Timer not assigned in ServerCommunicationManager");
        } 

        if (player == null) {
            GD.PrintErr("Player not assigned in ServerCommunicationManager");
        }
        else
        {
            var callable = new Callable(this, "OnDeathSignal");
            player.Connect(nameof(player.playerDeath), callable);
        }
    }

    private void OnDeathSignal(float deathPosX, float deathPosY)
    {
        SendDeath((float)levelTimer.timeLevelWasPlayedInSeconds, deathPosX, deathPosY);
    }

    public void SendRQ(float time, float posX, float posY)
    {
        SendDeathOrRQ("https://chroma-core.franek-stratovarius.duckdns.org/ragequit", time, posX, posY);
    }

    public void SendDeath(float time, float posX, float posY)
    {
        SendDeathOrRQ("https://chroma-core.franek-stratovarius.duckdns.org/death", time, posX, posY);
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

    public void SendWin(float time)
    {
        var dataAsDict = new Godot.Collections.Dictionary
        {
            { "time", time },
            { "level", level }
        };

        string message = Json.Stringify(dataAsDict);

        var httpRequest = new HttpRequest();
        AddChild(httpRequest);

        string url = "https://chroma-core.franek-stratovarius.duckdns.org/win";

        Error error = httpRequest.Request(url, null, Godot.HttpClient.Method.Post, message);
        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }


}
