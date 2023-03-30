using Godot;
using System;

public partial class PlayerController_2D : CharacterBody2D
{   
    public AnimatedSprite2D AnimatedSprite2D 
    {
        get;
        private set;
    }

    public State currentState;
    public State previousState;

    private Label velText;
    private Label stateText;
    private Label isOnFloorLabel;

    public override void _Ready()
    {
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        velText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("Velocity_Label");
        stateText = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("State_Label");
        isOnFloorLabel = GetNode<HFlowContainer>("HFlowContainer").GetNode<Label>("IsOnFloor_Label");
        ChangeState(new Falling(this));
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState.Execute(delta);
       
        velText.Text = "Velocity: " + Velocity.ToString();
        isOnFloorLabel.Text = "IsOnFloor: " + IsOnFloor();

        MoveAndSlide();
    }

    public void ChangeState(State newState)
    {
        previousState = currentState;
        currentState = newState;

        previousState?.Exit();
        currentState.Enter();

        stateText.Text = "State: " + currentState.Name;
    }

    public abstract class State
    {
        public abstract String Name { get; }

        protected PlayerController_2D _playerController2D;

        public State(PlayerController_2D playerController2D)
        {
            _playerController2D = playerController2D;
        }
        public abstract void Enter();
        public abstract void Execute(double delta);
        public abstract void Exit();
    }  

    public class Idle : State
    {
        public override string Name { get { return "Idle"; } }

        public Idle(PlayerController_2D controller) : base(controller) { }
        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("Idle");
        }
        public override void Execute(double delta)
        {
            if(CheckTransitionToJumping()) { return; }
            if(CheckTransitionToMoving()) { return; }
        }
        public override void Exit() { }

        private bool CheckTransitionToJumping()
        {
            var jumpPressedTrigger = Input.IsActionJustPressed("Jump");
            if (jumpPressedTrigger)
            {
                _playerController2D.ChangeState(new Jumping(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToMoving()
        {
            var axis = Input.GetAxis("Move_Left", "Move_Right");
            if (axis != 0) 
            {
                _playerController2D.ChangeState(new Moving(_playerController2D));
                return true; 
            }           
            return false;
        }
    }

    public class Moving : State
    {
        public override string Name { get { return "Move"; } }

        public static float MAX_MOVE_SPEED { get{ return 250f; } }
        public static float MOVE_SPEED_ACCELERATION { get { return 40f; } }
        public static float MOVE_SPEED_DECCELERATION { get { return 40f; } }

        private bool _movingLeft;

        public Moving(PlayerController_2D controller) : base(controller) { }
        public override void Enter()
        {
            _movingLeft = Input.IsActionPressed("Move_Left");
        }
        public override void Execute(double delta)
        {
            if (CheckTransitionToJumping()) { return; }
            if (CheckTransitionToDashing()) { return; }

            #region Movement
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");

            if (moveDirection != 0 && _playerController2D.IsOnFloor())
            {
                Acceleration(moveDirection);
            }
            else
            {
                Decerleration();
            }
            #endregion

            #region Animation
            // Left
            if (moveDirection < 0)
            {              
                _playerController2D.AnimatedSprite2D.Play("Walk");
                _playerController2D.AnimatedSprite2D.FlipH = true;
            }
            // Right
            else if(moveDirection > 0) 
            {
                _playerController2D.AnimatedSprite2D.Play("Walk");
                _playerController2D.AnimatedSprite2D.FlipH = false;
            }

            #endregion

            if(CheckTransitionToFalling()) { return; }
            if(CheckTransitionToIdle()) { return; }
        }
        public override void Exit() { }

        /// <summary>
        /// Applies a acceleration on the playerController.
        /// </summary>
        private void Acceleration(float direction)
        {           
            var velocity = _playerController2D.Velocity;
            velocity.X += MOVE_SPEED_ACCELERATION * direction;
            velocity.X = Mathf.Clamp(velocity.X, -MAX_MOVE_SPEED, MAX_MOVE_SPEED);
            _playerController2D.Velocity = velocity;
        }

        /// <summary>
        /// Applies a deceleration opposite to the playerControllers direction of movement
        /// </summary>
        private void Decerleration()
        {
            var velocity = _playerController2D.Velocity;
            if (_movingLeft)
            {
                velocity.X += MOVE_SPEED_DECCELERATION;
                velocity.X = Mathf.Clamp(velocity.X, -MAX_MOVE_SPEED, 0);
            }
            else
            {
                velocity.X -= MOVE_SPEED_DECCELERATION;
                velocity.X = Mathf.Clamp(velocity.X, 0, MAX_MOVE_SPEED);
            }
            _playerController2D.Velocity = velocity;
        }

        private bool CheckTransitionToJumping()
        {
            var jumpTrigger = Input.IsActionJustPressed("Jump");
            if (jumpTrigger)
            {
                _playerController2D.ChangeState(new Jumping(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDashing()
        {           
            var dashTrigger = Input.IsActionJustPressed("Dash");
            if (dashTrigger)
            {
                _playerController2D.ChangeState(new Dashing(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToFalling()
        {
            if (_playerController2D.Velocity.X == 0 &&
                    _playerController2D.IsOnFloor() == false)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToIdle()
        {
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            if (_playerController2D.Velocity.X == 0 &&
                    _playerController2D.IsOnFloor() &&
                        moveDirection == 0)
            {
                _playerController2D.ChangeState(new Idle(_playerController2D));
                return true;
            }
            return false;
        }
    }

    public class Jumping : State
    {
        public override string Name { get { return "Jumping"; } }

        public static float JUMP_SPEED { get { return 250f; } }
        public static float JUMP_END_MODIFIER { get { return 10f; } }

        public static bool ApexModifier { get; set; }

        public Jumping(PlayerController_2D playerController_2D) : base(playerController_2D) { }

        public override void Enter() { }

        public override void Execute(double delta)
        {
            Jump();

            if(CheckFallingTransition()) { return; }
        }
        public override void Exit() 
        {
            ApexModifier = true;
        }

        private void Jump()
        {
            var velocity = _playerController2D.Velocity;
            velocity.Y -= JUMP_SPEED;
            _playerController2D.Velocity = velocity;
        }
        private bool CheckFallingTransition()
        {
            if (_playerController2D.IsOnFloor() == false)
            {
                _playerController2D.ChangeState(new Falling(_playerController2D));
                return true;
            }
            return false;
        }
    }

    public class Falling : State
    {
        public override string Name { get { return "Falling"; } }

        public static float MAX_FALL_SPEED { get { return 700f; } }
        public static float FALL_SPEED_ACCELERATION { get { return 25f; } }
        public static float MAX_FALLING_MOVE_SPEED { get {return Moving.MAX_MOVE_SPEED * 0.9f; } }
        public static float FALLING_MOVE_SPEED_ACCELERATION { get { return Moving.MOVE_SPEED_ACCELERATION * 0.9f; } }

        private float _apexModifier = 0;
        private float _apex;

        public Falling(PlayerController_2D playerController_2D) : base(playerController_2D) { }

        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("InAir");
            _apexModifier = 1;
        }
        public override void Execute(double delta)
        {
            ApplyMovement();
            ApplyGravity();

            if (CheckTransitionToDash()) { return; }
            if (CheckTransitionToIdle()) { return; }
        }
        public override void Exit()
        {
            _playerController2D.Velocity = Vector2.Zero;
        }

        private void ApplyMovement()
        {
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            var velocity = _playerController2D.Velocity;

            velocity.X += FALLING_MOVE_SPEED_ACCELERATION * moveDirection;
            velocity.X = Mathf.Clamp(velocity.X, -MAX_FALLING_MOVE_SPEED, MAX_FALLING_MOVE_SPEED);

            _playerController2D.Velocity = velocity;
        }
        private void ApplyGravity()
        {
            var jumpEndTrigger = Input.IsActionJustReleased("Jump");
            var velocity = _playerController2D.Velocity;

            velocity.Y += (jumpEndTrigger) ? (FALL_SPEED_ACCELERATION * Jumping.JUMP_END_MODIFIER) : FALL_SPEED_ACCELERATION;
            velocity.Y = Mathf.Clamp(velocity.Y, float.MinValue, MAX_FALL_SPEED);

            if (_apex > velocity.Y && _playerController2D.previousState is Jumping) { GD.Print(velocity.Y); _apex = velocity.Y; }
            else if(_apex >= velocity.Y && _apex < 0) 
            {
                _apexModifier = 0;
                GD.Print("Apex!"); 
            }
            velocity.Y *= _apexModifier;
            _playerController2D.Velocity = velocity;
        }

        private async void WaitForApexModifier()
        {
            
        }

        private bool CheckTransitionToIdle()
        {
            if (_playerController2D.IsOnFloor())
            {
                _playerController2D.ChangeState(new Idle(_playerController2D));
                return true;
            }
            return false;
        }
        private bool CheckTransitionToDash()
        {
            var dashTrigger = Input.IsActionJustPressed("Dash");
            if(dashTrigger)
            {
                _playerController2D.ChangeState(new  Dashing(_playerController2D));
                return true;
            }
            return false;
        }
    }

    public class Dashing : State
    {
        public override string Name { get { return "Dash"; } }

        public static float DASH_SPEED { get{ return 1000f; } }

        public static float DASH_DURATION { get { return 0.2f; } }

        public Dashing(PlayerController_2D playerController) : base (playerController) { }
        public override void Enter()
        {                      
            if (CheckFallingOrMovingTransition()) { return; }

            Dash();
          
            WaitForTimer();
        }
        public override void Execute(double delta) { }

        public override void Exit()
        {
            _playerController2D.AnimatedSprite2D.FlipH = false;
        }
        private void Dash()
        {
            var direction = Input.GetAxis("Move_Left", "Move_Right");
            var velocity = _playerController2D.Velocity;
            velocity.X = DASH_SPEED * direction;
            _playerController2D.Velocity = velocity;

            #region Animation
            //Left
            if (direction < 0)
            {
                _playerController2D.AnimatedSprite2D.FlipH = true;
            }
            //Right
            else
            {
                _playerController2D.AnimatedSprite2D.FlipH = false;
            }
            _playerController2D.AnimatedSprite2D.Play("Dash");
            #endregion
        }

        /// <summary>
        /// Waits for the timer and changes then back to the previous state - falling or moving
        /// </summary>
        private async void WaitForTimer()
        {
            await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(DASH_DURATION), SceneTreeTimer.SignalName.Timeout);
            _playerController2D.ChangeState(_playerController2D.previousState);
        }

        private bool CheckFallingOrMovingTransition()
        {
            var direction = Input.GetAxis("Move_Left", "Move_Right");
            // To avoid Dash without direction
            if (direction == 0)
            {
                _playerController2D.ChangeState(_playerController2D.previousState);
                return true;
            }
            return false;
        }
    }
}
