using Godot;
using System;

public partial class DashCooldown : Control
{
    private Slider cooldownVisualisation;
    float cooldowtest = 10;
    float currentcooldown = 0;

    public override void _Ready()
    {
        cooldownVisualisation = (Slider)GetNode("DashCooldownSlider");
    }

    public override void _Process(double delta)
    {
        currentcooldown += (float)delta;
        UpdateDashSlider(currentcooldown, cooldowtest);
    }

    private void UpdateDashSlider(float currentTime, float totalCooldownTime)
    {
        cooldownVisualisation.Value = currentTime/totalCooldownTime;
    }
}
