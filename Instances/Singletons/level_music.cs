using Godot;
using System;

public partial class level_music : Node
{
	[Export]
	private string _music;
	private SoundManager _sound_manager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sound_manager = GetNode<SoundManager>("/root/SoundManager");
		_sound_manager.PlayMusic(_music);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
