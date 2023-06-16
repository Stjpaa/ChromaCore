using Godot;
using System;

public partial class AddParticles : Control
{
    [Export] private string particlesSceneToAdd;

    public void InstantiateParticles()
    {
        PackedScene particleScene = (PackedScene)ResourceLoader.Load(particlesSceneToAdd);
        AddChild(particleScene.Instantiate());
    }

    public void ClearParticles()
    {
        foreach (GpuParticles2D particle in GetChildren())
        {
            particle.QueueFree();
        }
    }
}
