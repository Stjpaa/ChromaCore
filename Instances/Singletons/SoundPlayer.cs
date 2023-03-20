using Godot;
using System;

public partial class SoundPlayer : Node
{
	public AudioStreamPlayer _audioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._audioPlayer = GetNode<AudioStreamPlayer>("AudioPlayers/Audioplayer");
	}

	public void PlaySound() 
	{
		this._audioPlayer.Play();
	}
}
