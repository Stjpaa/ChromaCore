using Godot;
using System;

public partial class SoundExample : Node
{
	private SoundManager sound_manager;

	public override void _Ready()
	{
		sound_manager = GetNode<SoundManager>("/root/SoundManager");
		sound_manager.PlaySound("Rise");
		sound_manager.PlayMusic("level_3");
		sound_manager.StopMusic();
	}
}
