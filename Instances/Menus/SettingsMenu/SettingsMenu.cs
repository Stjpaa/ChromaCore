using Godot;
using System;

public partial class SettingsMenu : Control
{
    private int masterIndex;
    public Control menuToReturnTo; // set when Oppening the SettingsMenu

    public override void _Ready()
    {
        masterIndex = AudioServer.GetBusIndex("Master");
    }

    public void _on_master_slider_value_changed(float sliderValue)
    {
        if(sliderValue == 0)    // Mute the bus if the slider is 0
        {
            AudioServer.SetBusMute(masterIndex, true);
            return;
        }

        AudioServer.SetBusMute(masterIndex, false);

        AudioServer.SetBusVolumeDb(masterIndex, sliderValue);




        //GD.Print(AudioServer.GetBusVolumeDb(masterIndex));
    }

    public void CloseSettingsMenu()     // called by the BackButton
    {
        this.Visible = false;

        if(menuToReturnTo == null)
        {
            GD.PrintErr("no menuToReturnTo was assigned when oppening the SettingsMenu");
            return;
        }

        menuToReturnTo.Visible = true;
    }

}
