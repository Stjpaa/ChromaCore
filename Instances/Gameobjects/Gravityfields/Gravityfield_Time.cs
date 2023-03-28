using Godot;
using System;

[Tool]
public partial class Gravityfield_Time : Area2D
{
	[Export]
	private Vector2 _gravityDirection;

	[Export]
	private float _gravityStrength = 200;

	[Export]
	private float _intervalTime = 3;

	[Export]
	private float _enabledTime = 3;

	[Signal]
	public delegate void OnGravityfieldEnteredEventHandler(Vector2 direction, float strength);

	[Signal]
	public delegate void OnGravityfieldExitedEventHandler();

	private Timer _intervalTimer;
	private Timer _enabledForTimer;

	private CollisionShape2D _gravityfieldCollider;

	private ShaderMaterial _spriteMat;

	public override void _Ready()
	{
		this._gravityfieldCollider = GetNode<CollisionShape2D>("GravityfieldCollider");
		this._gravityDirection = GetNode<Node2D>("GravityDirection").Position;
		this._intervalTimer = GetNode<Timer>("Interval");
		this._intervalTimer.Start(this._intervalTime);
		this._enabledForTimer = GetNode<Timer>("EnabledFor");

		this._spriteMat = GetNode<Sprite2D>("Sprite").Material as ShaderMaterial;
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		this._spriteMat.SetShaderParameter("particle_color", Colors.LawnGreen);
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
    {
		if (Engine.IsEditorHint())
		{
			DrawLine(new Vector2(0,0), this._gravityDirection, Colors.Blue, 0.3f);
		}
    }

	public void OnIntervalTimeout()
	{
		this._gravityfieldCollider.Disabled = false;
		this._enabledForTimer.Start(this._enabledTime);
		this._spriteMat.SetShaderParameter("pause", false);
	}

	public void OnEnabledForTimeout()
	{
		this._gravityfieldCollider.Disabled = true;
		this._intervalTimer.Start(this._intervalTime);
		this._spriteMat.SetShaderParameter("pause", true);
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
