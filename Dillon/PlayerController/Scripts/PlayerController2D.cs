using Godot;
using PlayerController.States;

namespace PlayerController
{
    public partial class PlayerController2D : CharacterBody2D
    {
        // GitHub Commit logs Version 0.0.18
        // Refactoring and documentation for the states of the grappling hook
        // - Idle - Shooting - Connected - Returning
        // Refactoring and documentation
        // - Grappling Hook - Grappling Hook.Physics -
        // 
        // Added a indicator icon for the hookable area
        // Added grappling hook data for balancing
        // Implemented grappling hook constraints
        // -> min max distance
        // -> only if the player is not on the ground
        // -> max upwards velocity on release
        // Implemented grappling hook velocity transition
        // Updated the collision masks for the checkpoint, traps etc.
        // Added new visuals for the grappling hook
        // Implemented interaction with traps
        // Implemented interaction between grappling hook and portals, gravity fiels and jump pad
        // When entering a checkpoint the position of the checkpoint is saved and not the position of the player
        //
        // Problems:
        // - Dash works not correctly inside a gravity field => open
        // - Gravity field is not effecting the player during moving state => open 

        [Export]
        public PlayerController2D_Data data;

        [Export]
        public GrapplingHook.GrapplingHook GrapplingHook
        {
            get;
            private set;
        }

        public AnimatedSprite2D AnimatedSprite2D
        {
            get;
            private set;
        }

        public Timer DashCooldownTimer
        {
            get;
            private set;
        }

        public Vector2 CheckpointPosition
        {
            get { return _checkpointPosition; }
            private set { _checkpointPosition = value; }
        }

        public Vector2 HookStartPosition
        {
            get { return _hookStartPositioNode.GlobalPosition; }
        }

        public bool HookIsReady { get; private set; }

        #region Moving State Variables
        public float MaxMoveSpeed { get { return data.maxMoveSpeed; } }
        public float MoveSpeedAcceleration { get { return data.moveSpeedAcceleration; } }
        public float MoveSpeedDecceleration { get { return data.moveSpeedDecceleration; } }
        #endregion
        #region Falling State Variables
        public float MaxFallSpeed { get { return data.maxFallSpeed; } }
        public float FallSpeedAcceleration { get { return data.fallSpeedAcceleration; } }
        public float MaxFallingMoveSpeed { get { return data.maxFallingMoveSpeed; } }
        public float FallingMoveSpeedAcceleration { get { return data.fallingMoveSpeedAcceleration; } }

        public float FallingMoveSpeedDeceleration { get { return data.FallingMoveSpeedDeceleration; }}
        #endregion
        #region Jumping State Variables
        public float JumpForce { get { return data.jumpForce; } }
        public float JumpEndModifier { get { return data.jumpEndModifier; } }
        #endregion
        #region Dashing State Variables
        public float DashSpeed { get { return data.dashSpeed; } }
        public float DashDuration { get { return data.dashDuration; } }
        #endregion
        #region Mechanics Variables
        public float CoyoteTimeDuration { get { return data.coyoteTimeDuration; } }
        public float ApexModifierDuration { get { return data.apexModifierDuration; } }
        public float ApexModifierMovementBoost { get { return data.apexModifierMovementBoost; } }
        #endregion

        public State currentState;
        public State previousState;      

        private Vector2 _checkpointPosition;

        private Timer _teleportTimer;

        private Node2D _hookStartPositioNode;

        private Node _parent;

        private Label velText;
        private Label stateText;
        private Label isOnFloorLabel;
        private Label floorNormalText;
        private Label floorAngleText;
        private Label dashCooldown;

        public override void _Ready()
        {
            AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");
            _teleportTimer = GetNode<Timer>("Teleport_Timer");
            _hookStartPositioNode = GetNode<Node2D>("HookStartPosition_Node2D");
            _parent = GetParent();

            velText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("Velocity_Label");
            stateText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("State_Label");
            isOnFloorLabel = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("IsOnFloor_Label");
            floorNormalText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorNormal_Label");
            floorAngleText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("FloorAngle_Label");
            dashCooldown = GetNode<Label>("Label");
            ChangeState(new Falling(this));
        }

        public override void _PhysicsProcess(double delta)
        {
            currentState.Execute();

            velText.Text = "Velocity: " + Velocity.ToString();
            isOnFloorLabel.Text = "IsOnFloor: " + IsOnFloor();
            floorNormalText.Text = "FloorNormal: " + GetFloorNormal();
            floorAngleText.Text = "FloorAngle: " + GetFloorAngle();

            dashCooldown.Text = string.Format("{0:N0}", DashCooldownTimer.TimeLeft);
            MoveAndSlide();
            CheckCollisionWithBox();
        }

        public void ChangeState(State newState)
        {
            previousState = currentState;
            currentState = newState;

            previousState?.Exit();
            currentState.Enter();

            stateText.Text = "State: " + currentState.Name;
        }

        #region Communication with other game objects
        public void SetParent(Node newParent)
        {
            Position = Vector2.Zero;
            GetParent().RemoveChild(this);
            newParent.AddChild(this);
        }

        public void ResetParent()
        {
            var globalPosition = GlobalPosition;
            GetParent().RemoveChild(this);
            _parent.AddChild(this);
            GlobalPosition = globalPosition;
        }

        public bool ShootGrapplingHook()
        {
            if(HookIsReady)
            {
                GrapplingHook.ShootHook(this);
            }
            return HookIsReady;
        }

        /// <summary>
        /// Called by gravity fields
        /// </summary>
        private void ChangeGravityProperties(Vector2 direction)
        {
            if(currentState is Hooking) 
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

            GD.Print("Gravity field left");
            ChangeState(new Falling(this));
        }

        /// <summary>
        /// Called by jump pads
        /// </summary>
        private void ApplyJumpPadForce(Vector2 strength)
        {
            if (currentState is Hooking)
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
            if (_teleportTimer.TimeLeft > 0) { return; }

            if (currentState is Hooking)
            {
                GrapplingHook.ReturnHook();
                GrapplingHook.Chain.Visible = false;
            }

            // Set the position to the teleport position
            Transform = new Transform2D(0, newPos);
            ChangeState(new Jumping(this, false, impulse));

            _teleportTimer.Start();
        }

        /// <summary>
        /// Called by traps
        /// </summary>
        private void TeleportToLastCheckPoint()
        {
            if (currentState is Hooking)
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
                    @object.ApplyCollisionImpulse(new Vector2(20 * Input.GetAxis("Move_Left", "Move_Right"), 0));
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
    }
}