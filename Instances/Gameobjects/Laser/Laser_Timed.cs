using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class Laser_Timed : Node2D
{
	[Export(PropertyHint.Range, "0.0,10,0.1")]
	private float _enabledTime = 1.0f;
	[Export(PropertyHint.Range, "0.0,10,0.1")]
	private float _pauseTime = 2.0f;
	[Export(PropertyHint.Range, "0.0,10,0.1")]
	private float _switchTime = 0.3f;

	private bool on = true;
	private float timer;

	private Node2D target;
	private ColorRect laser_rect;
	private ShaderMaterial shader_material;

	private void RecalculateLaser()
	{
		Vector2 target_vector = target.GetGlobalTransform().Origin - this.GetGlobalTransform().Origin;
		float laser_angle = target_vector.Angle();
		PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;
		PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(this.GetGlobalTransform().Origin, this.GetGlobalTransform().Origin + Vector2.FromAngle(laser_angle) * 1000);
		query.CollideWithAreas = true;
		Dictionary result = spaceState.IntersectRay(query);

		float path_length = 1000;
		
		if (result.Count > 0)
		{
			if (!Engine.IsEditorHint() && on)
			{
				try
				{
					PlayerController.PlayerController2D player_body = (PlayerController.PlayerController2D)result["collider"];
					if(player_body != null)
					{
						player_body.KilledPlayer();
					}
				}
				catch(InvalidCastException)
				{ /* do nothing */ }
			}

			Vector2 laser_origin = this.GetGlobalTransform().Origin;
			Vector2 laser_target_position = (Vector2)result["position"];
			Vector2 laser_path = laser_target_position - laser_origin;
			path_length = laser_path.Length();
		}

		laser_rect.Size = new Vector2(path_length, 30);
		laser_rect.Rotation = laser_angle;
		laser_rect.Position = new Vector2(0, -15);
	}

	public override void _Ready()
	{
		target = GetNode<Node2D>("Laser_Target");
		laser_rect = GetNode<ColorRect>("Laser_Rect");
		shader_material = laser_rect.Material as ShaderMaterial;
		RecalculateLaser();

		timer = _enabledTime;
	}

	public override void _Process(double delta)
	{
		RecalculateLaser();
		timer -= (float)delta;
		if (timer <= 0)
		{
			if (on)
			{
				on = false;
				timer = _pauseTime;
			}
			else
			{
				on = true;
				timer = _enabledTime;
			}
		}
		else
		{
			float progress = 0.0f;
			if (on)
			{
				progress = _enabledTime - timer;
			}
			else
			{
				progress = _switchTime - (_pauseTime - timer);
			}
			shader_material.SetShaderParameter("progress", Math.Clamp(progress, 0.0f, _switchTime) / _switchTime);
		}
	}
}
