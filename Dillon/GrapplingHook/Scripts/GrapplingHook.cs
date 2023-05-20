using Godot;
using System;
using PlayerController;
using GrapplingHook.States;
using GrapplingHook.Physics;
using System.Collections.Generic;
using System.Linq;

namespace GrapplingHook
{
    /// <summary>
    /// The grappling hook is seperated into two parts. One for the visuals and one for the physics. Both work independently.
    /// <para>
    /// The visuals is implemented as a Node with two childs. A sprite for the tip
    /// and a line2d node for the chain that holds two points.
    /// The first point is always at the tip of the hook and 
    /// the second point is updated during process to the player hook start position.
    /// </para>
    /// <para>
    /// The physics are controled in the states of the hook.
    /// See class physics for the implementation of the physics.
    /// </para>
    /// </summary>
    public partial class GrapplingHook : Node
    {
        [Export]
        public GrapplingHook_Data data;

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

        #region Balancing
        public float HookVelocityMultiplikatorOnRelease { get { return data.HookVelocityMultiplikatorOnRelease; } }
        public float MaxHookUpwardsVelocityOnRelease { get { return data.MaxHookUpwardsVelocityOnRelease; } }
        public float PlayerVelocityMultiplikatorOnStart { get { return data.PlayerVelocityMultiplikatorOnStart; } }
        public float MinDistance { get { return data.MinDistance; } }
        public float MaxDistance { get { return data.MaxDistance; } }
        public float StartImpulse { get { return data.MaxDistance; } }
        #endregion

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
        }
       
        #region Communication with the player controller
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

        /// <summary>
        /// Changes the state of the hook to returning
        /// </summary>
        public void ReturnHook()
        {
            if(_state is not Connected) { return; }

            ChangeState(new Returning(this));
        }
        #endregion

        #region Target
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

            // Get the closest target
            var minDistance = _targetList.Min(o => o.GlobalPosition.DistanceTo(GetHookStartPosition()));
            var closestTarget = from target in _targetList
                                where target.GlobalPosition.DistanceTo(GetHookStartPosition()) == minDistance &&
                                // Min Distance check
                                target.GlobalPosition.DistanceTo(GetHookStartPosition()) > MinDistance &&
                                // Max Distance check
                                target.GlobalPosition.DistanceTo(GetHookStartPosition()) <= MaxDistance &&
                                // Is on ground check
                                _playerController.IsOnFloor() == false
                                select target;

            _currentTarget = closestTarget.FirstOrDefault();

            GD.Print(minDistance);
        }
        #endregion

        #region Visuals
        /// <summary>
        /// Updates the second position of the line2d node.
        /// </summary>
        public void UpdateChainEndPosition()
        {
            var offset = GetCurrentHookLength();
            Chain.SetPointPosition(1, new Vector2(offset, 0));

            Chain.LookAt(_playerController.HookStartPosition);
        }

        private int GetCurrentHookLength()
        {
            return Mathf.RoundToInt(_playerController.HookStartPosition.DistanceTo(Hook.GlobalPosition));
        }
        #endregion

        #region Getters

        public Vector2 GetHookVelocityOnRelease()
        {
            var velocity = Physics.HookStart.LinearVelocity * HookVelocityMultiplikatorOnRelease;
            // Clamp the upwards movement
            if(velocity.Y < 0)
            {
                velocity.Y = Mathf.Clamp(velocity.Y, -MaxHookUpwardsVelocityOnRelease, float.MaxValue);
            }            
            return velocity;
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
        #endregion       
    }
}
