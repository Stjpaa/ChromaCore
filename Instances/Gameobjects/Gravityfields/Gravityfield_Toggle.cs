using Godot;
using System;

[Tool]
public partial class Gravityfield_Toggle : Node2D
{
	[Export]
	private Vector2 _gravityfieldSize = new Vector2(50, 50);

	[Export]
	private float _gravityStrength = 200;

	private Sprite2D _sprite;

	private ShaderMaterial _spriteMat;

	private CollisionShape2D _gravityfieldCollider;

	private bool _gravityfieldDisabled = true;

	private Node2D _gravityDirectionNode;

	private Vector2 _gravityDirection;

	private Area2D _switch;

	public override void _Ready()
	{
		// Get dependencies
		this._gravityfieldCollider = GetNode<CollisionShape2D>("Gravityfield/GravityfieldCollider");
		this._gravityDirectionNode = GetNode<Node2D>("Gravityfield/GravityDirection");
		this._sprite = GetNode<Sprite2D>("Gravityfield/Sprite");

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
		GD.Print("Entered");
		if (!this._gravityfieldDisabled)
		{
			body.Call("ChangeGravityProperties", this._gravityDirection.Normalized() * this._gravityStrength);
		}
	}	

	public void OnBodyExited(Node2D body)
	{
		if (!this._gravityfieldDisabled)
		{
			body.Call("ResetGravityProperties");			
		}
	}

	public void EnableGravityfield(Node2D body)
	{
		// Does not work with SetDeffered => probably bugged
		this._gravityfieldDisabled = false;
		this._spriteMat.SetShaderParameter("pause", false);
	}

	public void DisableGravityfield(Node2D body)
	{
		this._gravityfieldDisabled = true;
		this._spriteMat.SetShaderParameter("pause", true);
	}
}
