using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class SoundManager : Node
{
	[Export]
	private int _soundVoicesCount = 5;
	private int _randomVoicesCount = 5;

	[Export]
	private Godot.Collections.Dictionary<String, AudioStreamWav> _soundFiles = new Godot.Collections.Dictionary<String, AudioStreamWav>();
	[Export]
	private Godot.Collections.Dictionary<String, AudioStreamWav> _musicFiles = new Godot.Collections.Dictionary<String, AudioStreamWav>();
	[Export]
	public Godot.Collections.Array<AudioStreamWav> _teleporterSounds;
	[Export]
	public Godot.Collections.Array<AudioStreamWav> _buttonSounds;
	[Export]
	public Godot.Collections.Array<AudioStreamWav> _deathSounds;

	private AudioStreamPlayer music_player;
	private AudioStreamPlayer[] sound_players;
	private AudioStreamPlayer[] random_players;
	private int last_voice = 0;
	private int last_random_voice = 0;
	private Random rnd = new Random();

	public override void _Ready()
	{
		music_player = new AudioStreamPlayer();
		AddChild(music_player);
		sound_players = new AudioStreamPlayer[_soundVoicesCount];
		for (int i = 0; i < _soundVoicesCount; i++)
		{
			sound_players[i] = new AudioStreamPlayer();
			AddChild(sound_players[i]);
		}
		random_players = new AudioStreamPlayer[_randomVoicesCount];
		for (int i = 0; i < _randomVoicesCount; i++)
		{
			random_players[i] = new AudioStreamPlayer();
			AddChild(random_players[i]);
		}
	}

	public void PlaySound(String sound_name)
	{
		for (int i = 1; i <= _soundVoicesCount; i++)
		{
			if (!sound_players[(last_voice + i) % _soundVoicesCount].Playing)
			{
				sound_players[(last_voice + i) % _soundVoicesCount].Stream = _soundFiles[sound_name];
				sound_players[(last_voice + i) % _soundVoicesCount].Play();
				last_voice = (last_voice + i) % _soundVoicesCount;
				break;
			}

			// if all voices are playing, replace voice after last voice
			if (i == _soundVoicesCount)
			{
				sound_players[(last_voice + 1) % _soundVoicesCount].Stream = _soundFiles[sound_name];
				sound_players[(last_voice + 1) % _soundVoicesCount].Play();
				last_voice = (last_voice + 1) % _soundVoicesCount;
				break;
			}
		}
	}

	public void PlayMusic(String music_name)
	{
		music_player.Stream = _musicFiles[music_name];
		music_player.Play();
	}
	public void StopMusic()
	{
		music_player.Stop();
	}

	public void PlayRandom(Godot.Collections.Array<AudioStreamWav> sounds) {
		for (int i = 1; i <= _randomVoicesCount; i++)
		{
			if (!random_players[(last_random_voice + i) % _randomVoicesCount].Playing)
			{
				random_players[(last_random_voice + i) % _randomVoicesCount].Stream = GetSound(sounds);
				random_players[(last_random_voice + i) % _randomVoicesCount].Play();
				last_random_voice = (last_random_voice + i) % _randomVoicesCount;
				break;
			}

			// if all voices are playing, replace voice after last voice
			if (i == _randomVoicesCount)
			{
				random_players[(last_random_voice + 1) % _randomVoicesCount].Stream = GetSound(sounds);
				random_players[(last_random_voice + 1) % _randomVoicesCount].Play();
				last_random_voice = (last_random_voice + 1) % _randomVoicesCount;
				break;
			}
		}
	}


	private AudioStreamWav GetSound(Godot.Collections.Array<AudioStreamWav> sounds)
	{
		if(sounds.Count == 0) {
			return null;
		}
		int index = rnd.Next(sounds.Count);
		return sounds[index];
	}
}
