using Godot;
using System;

[Tool]
public partial class Gravityfield_Toggle : Area2D
{
	[Export]
	private Vector2 _gravityDirection;

	[Export]
	private float _gravityStrength = 200;

	[Signal]
	public delegate void OnGravityfieldEnteredEventHandler(Vector2 direction, float strength);

	[Signal]
	public delegate void OnGravityfieldExitedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 childPos = GetNode<Node2D>("GravityDirection").Position;
		this._gravityDirection = childPos;

		ShaderMaterial spriteMat = GetNode<Sprite2D>("Sprite").Material as ShaderMaterial;
		spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		spriteMat.SetShaderParameter("particle_color", Colors.NavyBlue);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
