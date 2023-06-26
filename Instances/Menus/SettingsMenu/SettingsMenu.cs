using Godot;
using System;

public partial class SettingsMenu : Control
{
    private int masterIndex;
    public Control menuToReturnTo; // set when Oppening the SettingsMenu

    private SoundSaveData soundSaveData;
    [Export]private RemapInputs remapInputs;

    [Export] private Slider masterSlider;

    public override void _Ready()
    {
        LoadSavedSettings();
        masterIndex = AudioServer.GetBusIndex("Master");
    }

    public override void _Process(double delta)       // Problem with 
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (this.Visible == true)   // if this menu is open when esc is pressed close this menu and go back to the last menu
            {
                CloseSettingsMenu();
            }
        }
    }

    public void SignalFullScreenCheckBox(bool checkBoxChecked)
    {
        ChangeToFullscreen(checkBoxChecked);
    }

    public void SignalMasterSliderValueChanged(float sliderValue)
    {
        soundSaveData.masterVolume = sliderValue;
        SaveSystem.SaveSoundSavedata(soundSaveData);
        if(sliderValue == 0)    // Mute the bus if the slider is 0
        {
            AudioServer.SetBusMute(masterIndex, true);
            return;
        }

        AudioServer.SetBusMute(masterIndex, false);

        float valueInDB = SliderfloatToDB(sliderValue);

        AudioServer.SetBusVolumeDb(masterIndex, valueInDB);
    }

    private float SliderfloatToDB(float sliderValue)
    {
        return Mathf.Log(sliderValue / 100) * 20;   // 100 = slider max value
    }

    public void CloseSettingsMenu()     // called by the BackButton
    {
        DisableMenu();

        if (menuToReturnTo == null)
        {
            GD.PrintErr("no menuToReturnTo was assigned when oppening the SettingsMenu");
            return;
        }

        menuToReturnTo.ProcessMode = ProcessModeEnum.Inherit;
        menuToReturnTo.Visible = true;
    }

    public void ChangeToFullscreen(bool changeToFullscreen)
    {
        if(changeToFullscreen) 
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else 
        { 
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }

    public void DisableMenu()
    {
        this.Visible = false;
        this.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void EnableMenu()
    {
        this.Visible = true;
        this.ProcessMode = ProcessModeEnum.Inherit;
    }

    private void LoadSavedSettings()
    {
        soundSaveData = SaveSystem.LoadSoundSavedata();

        masterSlider.Value = soundSaveData.masterVolume;
    }
}
