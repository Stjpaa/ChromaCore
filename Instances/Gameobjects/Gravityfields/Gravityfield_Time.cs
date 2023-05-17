using Godot;
using System;

[Tool]
public partial class Gravityfield_Time : Area2D
{
	[Export]
	private Vector2 _gravityfieldSize = new Vector2(50, 50);

	[Export]
	private float _gravityStrength = 200;

	[Export]
	private float _intervalTime = 3;

	[Export]
	private float _enabledTime = 3;
	
	[Export]
	private AudioStreamPlayer2D _audioPlayerOutside;
	[Export]
	private AudioStreamPlayer _audioPlayerInside;

	[Signal]
	public delegate void OnGravityfieldEnteredEventHandler(Vector2 direction);

	[Signal]
	public delegate void OnGravityfieldExitedEventHandler();

	private Timer _intervalTimer;
	private Timer _enabledForTimer;

	private Sprite2D _sprite;

	private ShaderMaterial _spriteMat;

	private CollisionShape2D _gravityfieldCollider;

	private Node2D _gravityDirectionNode;

	private Vector2 _gravityDirection;

	public override void _Ready()
	{
		// Get dependencies
		this._gravityfieldCollider = GetNode<CollisionShape2D>("GravityfieldCollider");
		this._gravityDirectionNode = this._gravityfieldCollider.GetNode<Node2D>("GravityDirection");
		this._sprite = this._gravityfieldCollider.GetNode<Sprite2D>("Sprite");
		this._intervalTimer = GetNode<Timer>("Interval");
		this._intervalTimer.Start(this._intervalTime);
		this._enabledForTimer = GetNode<Timer>("EnabledFor");

		// Initialize collider- and spritesize
		this._gravityfieldCollider.Shape.Set("size", this._gravityfieldSize);
		this._sprite.Scale = this._gravityfieldCollider.Shape.GetRect().Size;

		// Set direction of gravityfield
		this._gravityDirection = this._gravityDirectionNode.Position;

		// Set Shader Parameters
		this._spriteMat = this._sprite.Material as ShaderMaterial;
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		this._spriteMat.SetShaderParameter("width", this._gravityfieldCollider.Shape.GetRect().Size.X);
		this._spriteMat.SetShaderParameter("heigth", this._gravityfieldCollider.Shape.GetRect().Size.Y);
		this._sprite.Material = this._spriteMat;
		_audioPlayerOutside.Play();
		_audioPlayerInside.Play();
		_audioPlayerInside.VolumeDb = -100;
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._sprite.Scale = this._gravityfieldCollider.Shape.GetRect().Size;
			this._gravityfieldCollider.Shape.Set("size", this._gravityfieldSize);
			

			this._gravityDirection = this._gravityDirectionNode.Position;
			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			DrawLine(new Vector2(0,0), this._gravityDirection, Colors.Blue, 0.5f);
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
		body.Call("ChangeGravityProperties", this._gravityDirection.Normalized() * this._gravityStrength);
		try
		{
			PlayerController.PlayerController2D player_body = (PlayerController.PlayerController2D)body;
			if(player_body != null)
			{
				_audioPlayerInside.VolumeDb = 0;
				_audioPlayerOutside.VolumeDb = -100;
			}
		}
		catch(InvalidCastException)
		{ /* do nothing */ }
	}	

	public void OnBodyExited(Node2D body)
	{
		body.Call("ResetGravityProperties");
		try
		{
			PlayerController.PlayerController2D player_body = (PlayerController.PlayerController2D)body;
			if(player_body != null)
			{
				_audioPlayerOutside.VolumeDb = 0;
				_audioPlayerInside.VolumeDb = -100;
			}
		}
		catch(InvalidCastException)
		{ /* do nothing */ }
	}
}
