using Godot;
using System;

public partial class GrapplingHook : Node2D
{

    public Node2D Hook
    {
        get;
        private set;
    }

    public Sprite2D HookSprite
    {
        get;
        private set;
    }

    public Line2D Chain
    {
        get;
        private set;
    }

    private Node2D _parent;
    private Vector2 _globalHookPosition;
    private float _hookAngleInRad;

    private float _direction;
    private float _hookSpeed = 10f;
    private float _hookLenght = 50f;

    public override void _Ready()
    {
        try
        {
            _parent = (Node2D)GetParent();
        }
        catch { GD.Print("Failed to get the parent of the grappling hook"); }
        Hook = GetNode<Node2D>("Hook_Node2D");
        HookSprite = Hook.GetNode<Sprite2D>("Hook_Sprite2D");
        Chain = GetNode<Line2D>("Chain_Line2D");
    }

    public override void _Process(double delta)
    {
        //if(GetHookLenght() >= 50f) { return; }

        UpdateHookPosition();
    }

    public void SetHookPosition(Vector2 hookPosition)
    {
        _globalHookPosition = hookPosition;
     
        UpdateHookPosition();

        Hook.LookAt(_parent.Transform.Origin);
        Hook.Rotate(Mathf.DegToRad(180));
        _hookAngleInRad = Hook.Transform.Rotation;
    }

    private void UpdateHookPosition()
    {
        // Hook position must be transformed into the local coordinate space of the grappling hook
        var position = ToLocal(_globalHookPosition);
        Hook.Transform = new Transform2D(_hookAngleInRad, position);
        Chain.Transform = new Transform2D(0, position);

        UpdateChainPosition();
    }

    private void UpdateChainPosition()
    {
        // Get the chain lenght - both points must be in the same coordinate space
        var chainOffset = ToLocal(_parent.Transform.Origin) - Hook.Transform.Origin;
       
        SetChainPosition(chainOffset);  
    }

    private void SetChainPosition(Vector2 startPosition)
    {
        Chain.SetPointPosition(1, startPosition);
    }

    private float GetHookLenght()
    {
        return (Chain.GetPointPosition(1) - Chain.GetPointPosition(0)).Length();
    }
}
