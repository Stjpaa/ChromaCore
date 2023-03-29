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

	private ShaderMaterial _spriteMat;

	private CollisionShape2D _gravityfieldCollider;

	private Area2D _switch;

	public override void _Ready()
	{
		this._gravityDirection = GetNode<Node2D>("GravityDirection").Position;
		this._gravityfieldCollider = GetNode<CollisionShape2D>("GravityfieldCollider");

		Sprite2D sprite = GetNode<Sprite2D>("Sprite");
		this._spriteMat = sprite.Material.Duplicate() as ShaderMaterial;
		sprite.Material = this._spriteMat;
		
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		this._spriteMat.SetShaderParameter("particle_color", Colors.NavyBlue);
		this._spriteMat.SetShaderParameter("pause", true);

		Area2D Switch = GetNode<Area2D>("Switch");
		Switch.Connect("OnSwitchTriggered", new Callable(this, MethodName.EnableGravityfield));
		Switch.Connect("OnSwitchLeft", new Callable(this, MethodName.DisableGravityfield));
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

	public void EnableGravityfield()
	{
		SetDeferred("_gravityfieldCollider", false);
		// this._gravityfieldCollider.Disabled = false;
		this._spriteMat.SetShaderParameter("pause", false);
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		this._spriteMat.SetShaderParameter("particle_color", Colors.NavyBlue);
		this._spriteMat.SetShaderParameter("width", this.Scale.X * 10);
		this._spriteMat.SetShaderParameter("heigth", this.Scale.Y * 10);
	}

	public void DisableGravityfield()
	{
		SetDeferred("_gravityfieldCollider", true);
		// this._gravityfieldCollider.Disabled = true;
		this._spriteMat.SetShaderParameter("pause", true);
	}
}
