using Godot;
using System;

public partial class LevelRotatorTrigger : Node2D
{
	[Export]
	private LevelRotator _levelRotator;
	[Export]
	private bool _clockwise = true;
	private Area2D collider;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		collider = GetNode<Area2D>("Pressure_Plate_Collision");

		collider.BodyEntered += Rotate;
	}
	
	private void Rotate(Node2D _body)
	{
		GD.Print(_body);
		_levelRotator.Rotate(_clockwise);
	}
}
