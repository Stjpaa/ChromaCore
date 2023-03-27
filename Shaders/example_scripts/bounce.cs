using Godot;
using System;

public partial class bounce : Sprite2D
{
	ShaderMaterial shader_material;
	double bounce_time = 0.0;
	public override void _Ready()
	{
		shader_material = this.Material as ShaderMaterial;
	}

	public override void _Process(double delta)
	{
		if (bounce_time < 1.0) {
			bounce_time += delta;
		} else {
			bounce_time = 1.0;
		}

	// set in Project > Project Settings > Input Map
		if (Input.IsActionPressed("bounce"))
		{
			bounce_time = 0.0;
		}

		shader_material.SetShaderParameter("time", bounce_time);
	}
}

