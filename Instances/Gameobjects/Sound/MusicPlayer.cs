using Godot;
using System;

public partial class MusicPlayer : Node
{

	[Export]
	private AudioStreamWav _intro;
	[Export]
	private AudioStreamWav _loop;
	[Export(PropertyHint.Range, "-100.0,100,0.1")]
	private float _volume;
	private AudioStreamPlayer _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = new AudioStreamPlayer();
		AddChild(_player);
		_player.VolumeDb = _volume;
		_player.Stream = _intro;
		_player.Play();
		_player.Finished += StartLoop;
	}
	
	private void StartLoop()
	{
		_player.Stream = _loop;
		_player.Play();
	}
}
