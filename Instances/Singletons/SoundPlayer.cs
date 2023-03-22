using Godot;
using System;

public partial class SoundPlayer : Node
{
	public void PlaySound(string Soundname) 
	{
		GetNode<AudioStreamPlayer>(Soundname).Play();
	}
}
