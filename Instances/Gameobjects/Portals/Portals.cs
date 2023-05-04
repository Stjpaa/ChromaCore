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

    public override void _Ready()
    {
		// Get dependencies
		Area2D portal1 = GetNode<Area2D>("Portal1");
		Area2D portal2 = GetNode<Area2D>("Portal2");

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

		DrawLine(this._portal1LocalPos, this._portal2LocalPos, new Color(255, 0, 0, 0.2f), 0.9f, true);
	}

	public void TeleportToPortal1(Node2D body)
	{
		body.Call("Teleport", this._portal1GlobalPos, this._portal1Direction.Normalized() * this._portal1ImpulseStrength);
	}

	public void TeleportToPortal2(Node2D body)
	{
		body.Call("Teleport", this._portal2GlobalPos, this._portal2Direction.Normalized() * this._portal2ImpulseStrength);
	}
}
