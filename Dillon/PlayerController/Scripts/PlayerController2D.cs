using Godot;
using PlayerController.States;
using PlayerController.FollowingCamera;
using System;

namespace PlayerController
{
    public partial class PlayerController2D : CharacterBody2D
    {
        // GitHub Commit logs Version 0.0.22
        // Added player sprite atlas

        [Export]
        public PlayerController2D_Data data;
        
        [Export]
        public FollowingCamera.FollowingCamera FollowingCamera;

        [Export]
        public GrapplingHook.GrapplingHook GrapplingHook { get; private set; }

        public AnimatedSprite2D AnimatedSprite2D { get; private set; }

        public Timer DashCooldownTimer { get; private set; }

        public Timer TeleportTimer { get; private set; }

        public Vector2 CheckpointPosition
        {
            get { return _checkpointPosition; }
            private set { _checkpointPosition = value; }
        }

        public bool HookIsReady { get; private set; }

        public State PreviousState { get { return _previousState; } }

        private State _currentState;
        private State _previousState;
        

        private Vector2 _checkpointPosition;

        private Node2D _hookStartPositioNode;

        private Node _parent;

        #region Shader Variables
        private ShaderMaterial shader_material;
        private Vector2 start_position;
        #endregion

        #region Balancing
        // Moving state
        public float MaxMoveSpeed { get { return data.maxMoveSpeed; } }
        public float MoveSpeedAcceleration { get { return data.moveSpeedAcceleration; } }
        public float MoveSpeedDecceleration { get { return data.moveSpeedDecceleration; } }

        // Falling state variables
        public float MaxFallSpeed { get { return data.maxFallSpeed; } }
        public float FallSpeedAcceleration { get { return data.fallSpeedAcceleration; } }
        public float MaxFallingMoveSpeed { get { return data.maxFallingMoveSpeed; } }
        public float FallingMoveSpeedAcceleration { get { return data.fallingMoveSpeedAcceleration; } }

        public float FallingMoveSpeedDeceleration { get { return data.FallingMoveSpeedDeceleration; }}

        // Jumping state variables
        public float JumpForce { get { return data.jumpForce; } }
        public float JumpEndModifier { get { return data.jumpEndModifier; } }

        // Dashing state variables
        public float DashSpeed { get { return data.dashSpeed; } }
        public float DashDuration { get { return data.dashDuration; } }
        public float DashCooldown { get { return data.dashCooldown; } }

        // Mechanics variables
        public float CoyoteTimeDuration { get { return data.coyoteTimeDuration; } }
        public float ApexModifierDuration { get { return data.apexModifierDuration; } }
        public float ApexModifierMovementBoost { get { return data.apexModifierMovementBoost; } }

        // Object interaction variables
        public float BoxInteractionImpuls { get { return data.boxInteractionImpuls; } }
        #endregion

        public override void _Ready()
        {
            AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");
            DashCooldownTimer.WaitTime = DashCooldown;
            TeleportTimer = GetNode<Timer>("Teleport_Timer");
            _hookStartPositioNode = GetNode<Node2D>("HookStartPosition_Node2D");
            _parent = GetParent();

            ChangeState(new Falling(this));

            HookIsReady = true;

            shader_material = GetNode<CanvasLayer>("CanvasLayer2").GetNode<ColorRect>("ColorRect").Material as ShaderMaterial;
            start_position = Transform.Origin;
        }

        public override void _Process(double delta)
        {
            _currentState.ExecuteProcess();
        }

        public override void _PhysicsProcess(double delta)
        {
            _currentState.ExecutePhysicsProcess();

            MoveAndSlide();
            CheckCollisionWithBox();

            FollowingCamera.UpdatePosition(GlobalPosition);    
            shader_material.SetShaderParameter("position", Transform.Origin - start_position);       
        }

        public void ChangeState(State newState)
        {
            _previousState = _currentState;
            _currentState = newState;

            _previousState?.Exit();
            _currentState.Enter();

            GD.Print("Entered state: " + _currentState.Name);
        }

        #region Communication with other game objects
        public bool ShootGrapplingHook()
        {
            if(HookIsReady && GrapplingHook != null)
            {
                GrapplingHook.ShootHook(this);
            }
            return HookIsReady;
        }

        /// <summary>
        /// Called by laser
        /// </summary>
        public void KilledPlayer()
        {
            if (_currentState is Hooking)
            {
                GrapplingHook.ReturnHook();
            }
            Transform = new Transform2D(0, _checkpointPosition);
        }

        /// <summary>
        /// Called by gravity fields
        /// </summary>
        private void ChangeGravityProperties(Vector2 direction)
        {
            if(_currentState is Hooking) 
            {
                GrapplingHook.ReturnHook();
            }
            HookIsReady = false;

            GD.Print("Gravity field entered");
            ChangeState(new Falling(this, direction));
        }

        /// <summary>
        /// Called by gravity fields
        /// </summary>
        private void ResetGravityProperties()
        {
            HookIsReady = true;

            if(_currentState is Dashing) { return; }
            GD.Print("Gravity field left");
            ChangeState(new Falling(this));
        }

        /// <summary>
        /// Called by jump pads
        /// </summary>
        private void ApplyJumpPadForce(Vector2 strength)
        {
            if (_currentState is Hooking)
            {
                GrapplingHook.ReturnHook();
            }

            GD.Print("Jumppad entered");
            ChangeState(new Jumping(this, false, strength));
        }

        /// <summary>
        /// Called by portals
        /// </summary>
        private void Teleport(Vector2 newPos, Vector2 impulse)
        {           
            GD.Print("Teleport entered");
            if (TeleportTimer.TimeLeft > 0) { return; }

            if (_currentState is Hooking)
            {
                GrapplingHook.ReturnHook();
                GrapplingHook.Chain.Visible = false;
            }

            // Set the position to the teleport position
            Transform = new Transform2D(0, newPos);
            ChangeState(new Jumping(this, false, impulse));

            TeleportTimer.Start();
        }

        /// <summary>
        /// Called by traps
        /// </summary>
        private void Respawn()
        {
            if (_currentState is Hooking)
            {
                GrapplingHook.ReturnHook();
            }

            Transform = new Transform2D(0, _checkpointPosition);
        }

        private void CheckCollisionWithBox()
        {
            if (GetSlideCollisionCount() > 0)
            {
                if (GetLastSlideCollision().GetCollider() is Box)
                {
                    var @object = GetLastSlideCollision().GetCollider() as Box;
                    @object.ApplyCollisionImpulse(new Vector2(BoxInteractionImpuls * Input.GetAxis("Move_Left", "Move_Right"), 0));
                }
            }
        }

        /// <summary>
        /// Called by check points
        /// </summary>
        private void SaveCheckpointLocation(Vector2 checkpointPosition)
        {
            GD.Print("Checkpoint entered");
            _checkpointPosition = checkpointPosition;
        }   
        #endregion

        public Vector2 GetHookStartPosition()
        {
            return _hookStartPositioNode.GlobalPosition;
        }
    }
}