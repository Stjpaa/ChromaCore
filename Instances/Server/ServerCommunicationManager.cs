using Godot;
using System;

public partial class ServerCommunicationManager : Node2D
{
    [Export] private int level = 1;
    private float testTime = 100.5f;
    private float testX = 133.7f;
    private float testY = 420.69f;


    public override void _Ready()
    {
        SendDeath(testTime, testX, testY);
    }

    public void SendRQ(float time, float posX, float posY)
    {
        SendDeathOrRQ("https://chroma-core.franek-stratovarius.duckdns.org/ragequit", time, posX, posY);
    }

    public void SendDeath(float time, float posX, float posY)
    {
        SendDeathOrRQ("https://chroma-core.franek-stratovarius.duckdns.org/death", time, posX, posY);
    }

    private void SendDeathOrRQ(string url,float time, float posX, float posY)
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

        Error error = httpRequest.Request(url, null, HttpClient.Method.Post, message);
        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }

    public void SendWin(float time)
    {

    }
}
