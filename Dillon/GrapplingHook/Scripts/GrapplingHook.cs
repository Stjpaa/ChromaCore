using Godot;
using System;
using PlayerController;
using GrapplingHook.States;
using GrapplingHook.Physics;
using System.Collections.Generic;
using System.Linq;

namespace GrapplingHook
{
    public partial class GrapplingHook : Node
    {
        // Visuals
        public Node2D Hook { get; private set; }

        public Line2D Chain { get; private set; }

        // Physics
        public Physics.Physics Physics { get; private set; }

        // Variables
        private PlayerController2D _playerController;  
        
        private State _state;

        private List<Node2D> _targetList;
        private Node2D _currentTarget;

        // Events
        public event Action ShootEvent;
        public event Action ReleaseEvent;

        // Balancing
        public float HookVelocityMultiplikatorOnRelease { get { return 1.5f; } }
        public float PlayerVelocityMultiplikatorOnStart { get { return 1f; } }


        public override void _Ready()
        {
            Hook = GetNode<Node2D>("Visuals/Hook");
            Chain = GetNode<Line2D>("Visuals/Hook/Chain");
            Physics = GetNode<Physics.Physics>("Physics");
            
            _targetList = new List<Node2D>();

            ChangeState(new Idle(this));
        }

        public override void _Process(double delta)
        {
            _state.ExecuteProcess();
            UpdateChainEndPosition();
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
                _currentTarget = null;
            }

            _state?.Exit();
            _state = newState;
            _state.Enter();
            GD.Print("Hook changed state to " + newState.Name);
        }

        public void ShootHook(PlayerController2D playerController)
        {
            _playerController = playerController;

            SetGrapplingHookTarget();
            if (_currentTarget != null)
            {
                ShootEvent?.Invoke();
            }
            //TODO GH only possible if 
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

        public void AddTarget(Node2D target)
        {
            _targetList.Add(target);
        }
        public void RemoveTarget(Node2D target)
        {
            if (_targetList.Contains(target))
            {
                _targetList.Remove(target);
            }
        }

        /// <summary>
        /// Sets the current target to the one which is the closest to the player.
        /// </summary>
        private void SetGrapplingHookTarget()
        {
            if (_targetList.Count == 0)
            {
                _currentTarget = null;
                return;
            }

            var minDistance = _targetList.Min(o => o.GlobalPosition.DistanceTo(GetHookStartPosition()));
            var closestTarget = from target in _targetList
                                where target.GlobalPosition.DistanceTo(GetHookStartPosition()) == minDistance
                                select target;

            _currentTarget = closestTarget.FirstOrDefault();

            // Min distance check
            if(minDistance < 50)
            {
                _currentTarget = null;
            }
        }

        /// <summary>
        /// Updates the visuals of the chain.
        /// </summary>
        private void UpdateChainEndPosition()
        {
            var offset = GetCurrentHookLength();
            Chain.SetPointPosition(1, new Vector2(offset, 0));

            Chain.LookAt(_playerController.HookStartPosition);
        }
        #endregion

        #region Getters

        public Vector2 GetHookVelocityOnRelease()
        {
            return Physics.HookStart.LinearVelocity * HookVelocityMultiplikatorOnRelease;
        }

        public Vector2 GetPlayerVelocityOnStart()
        {
            return _playerController.Velocity * PlayerVelocityMultiplikatorOnStart;
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
            if(_currentTarget == null)
            {
                GD.PrintErr("Target is null");
                return Vector2.Zero;
            }
            return _currentTarget.GlobalPosition;
        }       

        private int GetCurrentHookLength()
         {
            return Mathf.RoundToInt(_playerController.HookStartPosition.DistanceTo(Hook.GlobalPosition));
        }
        #endregion       
    }
}
