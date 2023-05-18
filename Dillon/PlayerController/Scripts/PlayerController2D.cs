using Godot;
using PlayerController.States;

namespace PlayerController
{
    public partial class PlayerController2D : CharacterBody2D
    {
        // GitHub Commit logs Version 0.0.18
        // Refactoring for the grappling hook and for the states
        // 
        //
        // Updated the collision masks for the checkpoint, traps etc.
        // Added new visuals for the grappling hook
        // Implemented interaction with traps
        // When entering a checkpoint the position of the checkpoint will saved and not the player position 
        // Problems:
        // - Dash works not correctly inside a gravity field => open
        // - Gravity field is not effecting the player during moving state 

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

        private void ChangeGravityProperties(Vector2 direction)
        {
            GD.Print("Gravity field entered");
            ChangeState(new Falling(this, direction));
        }

        private void ResetGravityProperties()
        {
            GD.Print("Gravity field left");
            ChangeState(new Falling(this));
        }

        private void ApplyJumpPadForce(Vector2 strength)
        {
            GD.Print("Jumppad entered");
            ChangeState(new Jumping(this, false, strength));
        }

        private void Teleport(Vector2 newPos, Vector2 impulse)
        {
            GD.Print("Teleport entered");
            if (_teleportTimer.TimeLeft > 0) { return; }

            // Set the position to the teleport position
            Transform = new Transform2D(0, newPos);
            ChangeState(new Jumping(this, false, impulse));

            _teleportTimer.Start();
        }

        private void TeleportToLastCheckPoint()
        {
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

        private void SaveCheckpointLocation(Vector2 checkpointPosition)
        {
            GD.Print("Checkpoint entered");
            _checkpointPosition = checkpointPosition;
        }
        #endregion
    }
}