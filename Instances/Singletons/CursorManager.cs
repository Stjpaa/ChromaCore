using Godot;
using System;

public partial class CursorManager : Node
{
    /*
     Plan: maus verstecken und deaktivieren? wie aktiviert man sie wieder? wenn nicht deaktivieren vielleicht maus ui interaktion stoppen

    andere Option: Maus verstecken und auf den aktuellen button bewegen, sollte man vielleicht sowieso machen, damit maus auf dem letzten button rauskommt


    wenn man maus bewegt soll auf Mausbewegung zurückgeschalten werden, Focus wird entfernt/ ausgeblendet damit nicht 2 button gehighlighted sind.

    man müsste auf den letzten gehoverten button wieder focus setzten und maus ausblenden
     */


    public override void _Process(double delta)
    {
    }


}
