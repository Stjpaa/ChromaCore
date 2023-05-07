using Godot;
using System;
using PlayerController;
using GrapplingHook.States;
using GrapplingHook.Physics;

namespace GrapplingHook
{
    public partial class GrapplingHook : Node
    {
        // Visuals
        public Node2D Hook { get; private set; }

        public Line2D Chain { get; private set; }

        // Physics
        public Physics.Physics Physics { get; private set; }

        // References
        private PlayerController2D _playerController;

        private Node2D _target;

        // Variables
        private State _state;

        // Events
        public event Action ShootEvent;


        public override void _Ready()
        {
            Hook = GetNode<Node2D>("Visuals/Hook");
            Chain = GetNode<Line2D>("Visuals/Hook/Chain");
            Physics = GetNode<Physics.Physics>("Physics");
            
            ChangeState(new Idle(this));
        }

        public override void _Process(double delta)
        {
            UpdateChainPosition();
            
        }

        public override void _PhysicsProcess(double delta)
        {
            _state.Execute();
            //_playerController.GlobalPosition = Physics.ChainStart.GlobalPosition;
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
            _playerController = playerController;
            if (_target != null)
            {
                ShootEvent?.Invoke();
            }
        }

        public void SetTarget(Node2D target)
        {
            if(target == _target) { _target = null; }
            else { _target = target; }
        }

        public Vector2 GetHookStartPosition()
        {
            if(_playerController == null)
            {
                GD.PrintErr("PlayerController is null");
                return Vector2.Zero;
            }
            return _playerController.HookStartPosition;
        }

        public Vector2 GetHookTargetPosition()
        {
            if(_target == null)
            {
                GD.PrintErr("Target is null");
                return Vector2.Zero;
            }
            return _target.GlobalPosition;
        }

        private int GetCurrentHookLength()
         {
            return Mathf.RoundToInt(_playerController.HookStartPosition.DistanceTo(Hook.GlobalPosition));
        }

        private void UpdateChainPosition()
        {
            var offset = GetCurrentHookLength();
            Chain.SetPointPosition(1, new Vector2(offset, 0));

            Chain.LookAt(_playerController.HookStartPosition);
        }
    }
}
