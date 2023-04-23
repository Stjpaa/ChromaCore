using Godot;
using System;

/*
	small testscript to trigger sounsa/music with keypresses
*/
public partial class Test : Node
{
	SoundManager sound_manager;
	
	 public override void _Ready()
	{
		sound_manager = GetNode<SoundManager>("/root/SoundManager");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.S)
			{
				sound_manager.PlaySound("Rise");
			}
			else if (keyEvent.Keycode == Key.D)
			{
				sound_manager.PlaySound("level_3");
			}
			else if (keyEvent.Keycode == Key.M)
			{
				sound_manager.PlayMusic("level_3");
			}
		}
	}
}
