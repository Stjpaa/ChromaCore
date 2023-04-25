using Godot;
using System;
using System.Collections.Generic;
using PlayerController;
using GrapplingHook.States;

namespace GrapplingHook {

    public partial class GrapplingHook : Node2D
    {
        [Export]
        public PackedScene grapplingHookJB;

        private Node2D _hook;

        private Sprite2D _hookSprite;

        private Line2D _chain;

        public PlayerController2D PlayerController
        {
            get;
            private set;
        }

        public Vector2 HookStartPosition
        {
            get { return PlayerController.HookStartPosition; }
        }

        public float HookSpeed { get { return 1500f; } }
        public float HookLenght { get { return 250f; } }

        public State _state;

        public Vector2 _globalTargetPosition;
        public Vector2 _direction;

        public event Action shootEvent;

        public List<RayCast2D> rayCast2Ds;

        public override void _Ready()
        {
            _hook = GetNode<Node2D>("Hook_Node2D");
            _hookSprite = _hook.GetNode<Sprite2D>("Hook_Sprite2D");
            _chain = GetNode<Line2D>("Chain_Line2D");
            rayCast2Ds = new List<RayCast2D>();
            rayCast2Ds.Add(GetNode<RayCast2D>("Top_RayCast2D"));
            rayCast2Ds.Add(GetNode<RayCast2D>("Bottom_RayCast2D"));

            ChangeState(new Idle(this));
        }

        public override void _Process(double delta)
        {
            _state.Execute();
            UpdateChainPosition();
        }

        public override void _PhysicsProcess(double delta)
        {
            
        }

        public void ChangeState(State newState)
        {
            _state?.Exit();
            _state = newState;
            _state.Enter();
            GD.Print("Hook changed state to " + newState.Name);
        }

        public void ShootHook(PlayerController2D playerController)
        {
            PlayerController = playerController;     
            _globalTargetPosition = PlayerController.GetGlobalMousePosition();
            shootEvent?.Invoke();
        }

        private void UpdateChainPosition()
        {
            // Get the chain lenght - both points must be in the coordinate space of the chain
            var chainOffset = _chain.ToLocal(PlayerController.HookStartPosition) - ToLocal(Transform.Origin);
            _chain.SetPointPosition(1, chainOffset);
        }

        public float GetCurrentHookLenght()
        {
            return ((_chain.GetPointPosition(1) + _chain.GlobalPosition) - GlobalPosition).Length();
        }

    }  
}
