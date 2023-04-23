using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class SoundManager : Node
{
	[Export]
	private int _soundVoicesCount = 5;

	[Export]
	private Godot.Collections.Dictionary<String, AudioStreamOggVorbis> _soundFiles = new Godot.Collections.Dictionary<String, AudioStreamOggVorbis>();
	[Export]
	private Godot.Collections.Dictionary<String, AudioStreamOggVorbis> _musicFiles = new Godot.Collections.Dictionary<String, AudioStreamOggVorbis>();

	private AudioStreamPlayer2D music_player;
	private AudioStreamPlayer2D[] sound_players;
	private int last_voice = 0;

	public override void _Ready()
	{
		music_player = new AudioStreamPlayer2D();
		AddChild(music_player);
		sound_players = new AudioStreamPlayer2D[_soundVoicesCount];
		for (int i = 0; i < _soundVoicesCount; i++)
		{
			sound_players[i] = new AudioStreamPlayer2D();
			AddChild(sound_players[i]);
		}
	}

	public void PlaySound(String sound_name)
	{
		for (int i = 1; i <= _soundVoicesCount; i++)
		{
			// GD.Print($"testing {(last_voice + i) % _soundVoicesCount}");
			if (!sound_players[(last_voice + i) % _soundVoicesCount].Playing)
			{
				// GD.Print($"playing {sound_name} with voice {(last_voice + i) % _soundVoicesCount}; last voice {last_voice}");
				sound_players[(last_voice + i) % _soundVoicesCount].Stream = _soundFiles[sound_name];
				sound_players[(last_voice + i) % _soundVoicesCount].Play();
				last_voice = (last_voice + i) % _soundVoicesCount;
				break;
			}

			// if all voices are playing, replace voice after last voice
			if (i == _soundVoicesCount)
			{
				// GD.Print($"replacing and playing {sound_name} with voice {(last_voice + 1) % _soundVoicesCount}; last voice {last_voice}");
				sound_players[(last_voice + 1) % _soundVoicesCount].Stream = _soundFiles[sound_name];
				sound_players[(last_voice + 1) % _soundVoicesCount].Play();
				last_voice = (last_voice + 1) % _soundVoicesCount;
				break;
			}
		}
	}

	public void PlayMusic(String music_name)
	{
		// GD.Print($"playing {music_name}");
		music_player.Stream = _musicFiles[music_name];
		music_player.Play();
	}
}
