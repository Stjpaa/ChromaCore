using Godot;
using System;

[Tool]
public partial class Gravityfield_Normal : Area2D
{
	[Export]
	private Vector2 _gravityDirection;

	[Export]
	private float _gravityStrength = 300;

	[Signal]
	public delegate void OnGravityfieldEnteredEventHandler(Vector2 direction, float strength);

	[Signal]
	public delegate void OnGravityfieldExitedEventHandler();

	private ShaderMaterial _spriteMat;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get position of child to determine the direction of the field
		Vector2 childPos = GetNode<Node2D>("GravityDirection").Position;
		this._gravityDirection = childPos;

		// Set Shader Parameters
		Sprite2D sprite = GetNode<Sprite2D>("Sprite");
		this._spriteMat = sprite.Material.Duplicate() as ShaderMaterial;
		sprite.Material = this._spriteMat;
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		// this._spriteMat.SetShaderParameter("particle_color", Colors.IndianRed);
		this._spriteMat.SetShaderParameter("width", this.Scale.X * 10);
		this._spriteMat.SetShaderParameter("heigth", this.Scale.Y * 10);
	}

	public override void _Process(double delta)
	{
		this._gravityDirection = GetNode<Node2D>("GravityDirection").Position;
		QueueRedraw();
	}

    public override void _Draw()
    {
		if (Engine.IsEditorHint())
		{
			DrawLine(new Vector2(0,0), this._gravityDirection, Colors.Blue, 0.3f);
		}
    }

	public void OnBodyEntered(Node2D body)
	{
		EmitSignal("OnGravityfieldEntered", this._gravityDirection, this._gravityStrength);
	}	

	public void OnBodyExited(Node2D body)
	{
		EmitSignal("OnGravityfieldExited");
	}
}
