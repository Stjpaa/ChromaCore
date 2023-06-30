using Godot;
using System;
using System.Threading.Tasks;

public partial class RagequitHandler : Node2D
{
    /*
    bool currentlyQuitting = false;
    [Export] public ServerCommunicationManager serverCommunication;
    public override void _Ready()
    {
        GetTree().AutoAcceptQuit = false;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            QuitGame();
        }
    }


    private void QuitGame()
    {
        if (currentlyQuitting)
        {
            return;
        }

        currentlyQuitting = true;
        GD.Print("Quit game via RagequitHandler");
        _ = QuitAfterServerUpdate();

    }

    private async Task QuitAfterServerUpdate()
    {
        serverCommunication.RQSignal();

        currentlyQuitting = false;

        GetTree().Quit();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            var eventkey = (InputEventKey)@event;

            if (eventkey.KeyLabel == Key.Escape)
            {
                GetTree().Quit();
            }
        }
    }
    */
}
