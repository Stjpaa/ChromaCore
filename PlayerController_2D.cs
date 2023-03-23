using Godot;
using System;

public partial class PlayerController_2D : CharacterBody2D
{
    private const float GRAVITY = 1500;
    private const float MOVE_SPEED = 250;
    
    public bool gravity = true;

    public AnimatedSprite2D AnimatedSprite2D 
    {
        get;
        private set;
    }

    public State currentState;
    public State previousState;

    public override void _Ready()
    {
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ChangeState(new Idle(this));
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        // Add gravity
        velocity.Y += (float)delta * GRAVITY;
        if (gravity == false) { velocity.Y = 0; }
        Velocity = velocity;

        currentState.Execute();

        GD.Print(IsOnFloor());

        MoveAndSlide();                                      
    }

    public void ChangeState(State newState)
    {
        previousState = currentState;
        currentState = newState;

        previousState?.Exit();
        currentState.Enter();

        GD.Print("Changed state");
    }

    public abstract class State
    {
        public State(PlayerController_2D playerController2D) 
        { 
            _playerController2D = playerController2D;
        }
        protected PlayerController_2D _playerController2D;
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }

    public class Idle : State
    {   
        public Idle(PlayerController_2D controller) : base(controller) { }
        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("Idle");
        }
        public override void Execute()
        {
            var jumpTrigger = Input.IsActionJustPressed("Jump");
            if(jumpTrigger)
            {
                _playerController2D.ChangeState(new Jump(_playerController2D));
            }

            var axis = Input.GetAxis("Move_Left", "Move_Right");
            if(axis == 0) { return; }

            _playerController2D.ChangeState(new Move(_playerController2D));

        }
        public override void Exit() { }
    }

    public class Move : State
    {
        private const float MOVE_SPEED = 250;
        public Move(PlayerController_2D controller) : base (controller) 
        {
        
        }
        public override void Enter()
        {
        
        }
        public override void Execute()
        {
            var moveDirection = Input.GetAxis("Move_Left", "Move_Right");
            var jumpTrigger = Input.IsActionJustPressed("Jump");
            var dashTrigger = Input.IsActionJustPressed("Dash");

            var velocity = _playerController2D.Velocity;
            velocity.X = MOVE_SPEED * moveDirection;
            _playerController2D.Velocity = velocity;

            if (jumpTrigger)
            {
                _playerController2D.ChangeState(new Jump(_playerController2D));
            }
            if (dashTrigger)
            {
                _playerController2D.ChangeState(new Dash(_playerController2D));
            }

            
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
            else
            {
                _playerController2D.ChangeState(new Idle(_playerController2D));
            }
        }
        public override void Exit()
        {

        }
    }

    public class Jump : State
    {
        private const float JUMP_SPEED = -500;

        public Jump(PlayerController_2D playerController) : base (playerController) { }
        public override void Enter()
        {
            GD.Print("jump");
            var velocity = _playerController2D.Velocity;
            velocity.Y = JUMP_SPEED;
            _playerController2D.Velocity = velocity;

            _playerController2D.ChangeState(_playerController2D.previousState);
        }
        public override void Execute()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    public class Dash : State
    {
        private const float DASH_SPEED = 6000;

        public Dash(PlayerController_2D playerController) : base (playerController) { }
        public override void Enter()
        {
            var velocity = _playerController2D.Velocity;
            velocity.X = DASH_SPEED;
            if(Input.GetAxis("Move_Left", "Move_Right") < 0)
            {
                velocity.X *= -1;
            }
            _playerController2D.Velocity = velocity;

            _playerController2D.AnimatedSprite2D.Play("Dash");
        }
        public override void Execute()
        {   
            _playerController2D.ChangeState(_playerController2D.previousState);
        }
        public override void Exit()
        {

        }
    }
}
