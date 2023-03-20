using Godot;
using System;

public partial class MovingPlatform : Node2D
{
	private SoundPlayer _audioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Move");
		this._audioPlayer = GetNode<SoundPlayer>("/root/SoundPlayer");
	}

	private void Move(float delta)
	{
		
	}
}
