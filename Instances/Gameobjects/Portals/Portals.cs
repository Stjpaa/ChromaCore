using Godot;
using System;

[Tool]
public partial class Portals : Node2D
{
	private Vector2 _portal1GlobalPos;

	private Vector2 _portal1LocalPos;

	private Vector2 _portal1Direction;

	[Export]
	private float _portal1ImpulseStrength = 300;

	private Vector2 _portal2GlobalPos;

	private Vector2 _portal2LocalPos;

	private Vector2 _portal2Direction;

	[Export]
	private float _portal2ImpulseStrength = 300;

	[Signal]
	public delegate void TeleportObjectEventHandler(Vector2 teleportTo, Vector2 impulse);

	private AudioStreamPlayer2D player_portal1;
	private AudioStreamPlayer2D player_portal2;
	private Random rnd = new Random();
	private SoundManager sound_manager;

	public override void _Ready()
	{
		// Get dependencies
		Area2D portal1 = GetNode<Area2D>("Portal1");
		Area2D portal2 = GetNode<Area2D>("Portal2");

		player_portal1 = GetNode<Area2D>("Portal1").GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		player_portal2 = GetNode<Area2D>("Portal2").GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		sound_manager = GetNode<SoundManager>("/root/SoundManager");

		// Get teleport positions
		this._portal1GlobalPos = portal1.GlobalPosition;
		this._portal2GlobalPos = portal2.GlobalPosition;

		// Get data for drawing
		this._portal1GlobalPos = portal1.GlobalPosition;
		this._portal2GlobalPos = portal2.GlobalPosition;
		this._portal1Direction = portal1.GetNode<Node2D>("ImpulseDirection").Position;
		this._portal2Direction = portal2.GetNode<Node2D>("ImpulseDirection").Position;
		this._portal1LocalPos = GetNode<Area2D>("Portal1").Position;
		this._portal2LocalPos = GetNode<Area2D>("Portal2").Position;
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._portal1LocalPos = GetNode<Area2D>("Portal1").Position;
			this._portal2LocalPos = GetNode<Area2D>("Portal2").Position;
			this._portal1Direction = GetNode<Area2D>("Portal1").GetNode<Node2D>("ImpulseDirection").Position;
			this._portal2Direction = GetNode<Area2D>("Portal2").GetNode<Node2D>("ImpulseDirection").Position;
		}

		QueueRedraw();
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			DrawLine(this._portal1LocalPos, this._portal1LocalPos + this._portal1Direction, Colors.Blue, 0.5f);
			DrawLine(this._portal2LocalPos, this._portal2LocalPos + this._portal2Direction, Colors.Blue, 0.5f);
		}

		DrawLine(this._portal1LocalPos, this._portal2LocalPos, new Color(255, 0, 255, 0.2f), 0.9f, true);
	}

	public void TeleportToPortal1(Node2D body)
	{
		body.Call("Teleport", this._portal1GlobalPos, this._portal1Direction.Normalized() * this._portal1ImpulseStrength);
		player_portal1.Stream = GetSound();
		player_portal1.Play();
	}

	public void TeleportToPortal2(Node2D body)
	{
		body.Call("Teleport", this._portal2GlobalPos, this._portal2Direction.Normalized() * this._portal2ImpulseStrength);
		player_portal2.Stream = GetSound();
		player_portal2.Play();
	}

	private AudioStreamWav GetSound()
	{
		Godot.Collections.Array<AudioStreamWav> _teleportSounds = sound_manager._teleporterSounds;
		if(_teleportSounds.Count == 0) {
			return null;
		}
		int index = rnd.Next(_teleportSounds.Count);
		return _teleportSounds[index];
	}
}
