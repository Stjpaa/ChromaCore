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

        // Variables
        private State _state;

        private Node2D _target;

        // Events
        public event Action ShootEvent;
        public event Action ReleaseEvent;


        public override void _Ready()
        {
            Hook = GetNode<Node2D>("Visuals/Hook");
            Chain = GetNode<Line2D>("Visuals/Hook/Chain");
            Physics = GetNode<Physics.Physics>("Physics");
            
            ChangeState(new Idle(this));
        }

        public override void _Process(double delta)
        {
            _state.ExecuteProcess();
            UpdateChainPosition();
        }

        public override void _PhysicsProcess(double delta)
        {
            _state.ExecutePhysicsProcess();
        }          

        public void ChangeState(State newState)
        {
            if(_state is Connected)
            {
                ReleaseEvent?.Invoke();
            }

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

        private void UpdateChainPosition()
        {
            var offset = GetCurrentHookLength();
            Chain.SetPointPosition(1, new Vector2(offset, 0));

            Chain.LookAt(_playerController.HookStartPosition);
        }

        public void SetTarget(Node2D target)
        {
            _target = target;
        }


        #region Methods
        /// <summary>
        /// Makes the player node to a child of the grappling hook
        /// </summary>
        public void SetPlayerAsChild()
        {
            _playerController.SetParent(Physics.HookStart);
        }

        public void RemovePlayerAsChild()
        {
            _playerController.ResetParent();
        }

        public void ChangePlayerStateToHooking()
        {
            _playerController.ChangeState(new PlayerController.States.Hooking(_playerController, this));
        }
        #endregion

        #region Getters

        public Node2D GetTarget()
        {
            return _target;
        }

        public Vector2 GetHookVelocityOnRelease()
        {
            return Physics.HookStart.LinearVelocity * 1.5f;
        }

        public Vector2 GetPlayerVelocity()
        {
            return _playerController.Velocity;
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
        #endregion       
    }
}
