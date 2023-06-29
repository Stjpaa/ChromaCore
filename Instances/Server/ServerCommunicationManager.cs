using Godot;
using System;

public partial class ServerCommunicationManager : Node2D
{
    [Export] private int level = 0;
    private float testTime = 100.5f;
    private float testX = 133.7f;
    private float testY = 420.69f;


    public override void _Ready()
    {
        Test();
        Test();
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
        //level = level;
        //Json json = new Json().ToString{ 
        //{
        //    "time":921769, // milisekunden
        //    "level":1,
        //    "positionx":20,
        //    "positiony":40
        //}
        //}

    }

    public void SendWin(float time)
    {

    }


    private void Test()
    {
        var httpRequest = new HttpRequest();
        AddChild(httpRequest);
        httpRequest.RequestCompleted += HttpRequestCompleted;

        // Perform a GET request. The URL below returns JSON as of writing.
        Error error = httpRequest.Request("https://httpbin.org/get");
        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }

        //// Perform a POST request. The URL below returns JSON as of writing.
        //// Note: Don't make simultaneous requests using a single HTTPRequest node.
        //// The snippet below is provided for reference only.
        //Json msg = new Json();
        //string body = msg.Stringify(new Godot.Collections.Dictionary{{ "name", "Godette" }});
        //error = httpRequest.Request("https://httpbin.org/post", null, true, HttpClient.Method.Post, body);
        //if (error != Error.Ok)
        //{
        //    GD.PushError("An error occurred in the HTTP request.");
        //}
    }

    // Called when the HTTP request is completed.
    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        var json = new Json();
        json.Parse(body.GetStringFromUtf8());
        //var response = json.GetData().AsGodotDictionary();
        var response = json.Data.AsGodotDictionary();

        //// Will print the user agent string used by the HTTPRequest node (as recognized by httpbin.org).
        GD.Print("request completed: " + (response["headers"].AsGodotDictionary())["User-Agent"]);
    }
}
