using Godot;
using System;

[Tool]
public partial class Gravityfield_Toggle : Area2D
{
	[Export]
	private Vector2 _gravityfieldSize = new Vector2(50, 50);

	[Export]
	private float _gravityStrength = 200;

	[Signal]
	public delegate void OnGravityfieldEnteredEventHandler(Vector2 direction);

	[Signal]
	public delegate void OnGravityfieldExitedEventHandler();

	private Sprite2D _sprite;

	private ShaderMaterial _spriteMat;

	private CollisionShape2D _gravityfieldCollider;

	private Node2D _gravityDirectionNode;

	private Vector2 _gravityDirection;

	private Area2D _switch;

	public override void _Ready()
	{
		// Get dependencies
		this._gravityfieldCollider = GetNode<CollisionShape2D>("GravityfieldCollider");
		this._gravityDirectionNode = this._gravityfieldCollider.GetNode<Node2D>("GravityDirection");
		this._sprite = this._gravityfieldCollider.GetNode<Sprite2D>("Sprite");

		// Set collider- and spritesize
		this._gravityfieldCollider.Shape.Set("size", this._gravityfieldSize);
		this._sprite.Scale = this._gravityfieldCollider.Shape.GetRect().Size;

		// Set direction of gravityfield
		this._gravityDirection = this._gravityDirectionNode.Position;

		// Set shader parameters
		this._spriteMat = this._sprite.Material as ShaderMaterial;
		this._spriteMat.SetShaderParameter("direction", -this._gravityDirection.Normalized());
		this._spriteMat.SetShaderParameter("strength", this._gravityStrength / 150);
		this._spriteMat.SetShaderParameter("width", this._gravityfieldCollider.Shape.GetRect().Size.X);
		this._spriteMat.SetShaderParameter("heigth", this._gravityfieldCollider.Shape.GetRect().Size.Y);
		this._sprite.Material = this._spriteMat;

		// Connect signals for switch
		Area2D Switch = GetNode<Area2D>("Switch");
		Switch.Connect("OnSwitchTriggered", new Callable(this, MethodName.EnableGravityfield));
		Switch.Connect("OnSwitchLeft", new Callable(this, MethodName.DisableGravityfield));
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

	public void OnBodyEntered(Node2D body)
	{
		EmitSignal("OnGravityfieldEntered", this._gravityDirection * this._gravityStrength);
	}	

	public void OnBodyExited(Node2D body)
	{

		EmitSignal("OnGravityfieldExited");
	}

	public void EnableGravityfield()
	{
		SetDeferred("_gravityfieldCollider", false);
		this._spriteMat.SetShaderParameter("pause", false);
	}

	public void DisableGravityfield()
	{
		SetDeferred("_gravityfieldCollider", true);
		this._spriteMat.SetShaderParameter("pause", true);
	}
}
