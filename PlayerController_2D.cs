using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static PlayerController_2D;

public partial class PlayerController_2D : CharacterBody2D
{   
    public bool gravity = true;
    private const float GRAVITY = 1500;

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

        velocity.Y += (float)delta * GRAVITY;
        Velocity = velocity;

        currentState.Execute(delta);

        MoveAndSlide();                                      
    }

    public void ChangeState(State newState)
    {
        previousState = currentState;
        currentState = newState;

        previousState?.Exit();
        currentState.Enter();

        GD.Print("Changed state to " + currentState.Name);
    }

    public abstract class State
    {
        public abstract String Name { get; }

        protected PlayerController_2D _playerController2D;
        protected List<Modificator> _modificators;

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

            var jumpTrigger = Input.IsActionJustPressed("Jump");
            if(jumpTrigger)
            {
                _playerController2D.ChangeState(new VariableJump(_playerController2D));
            }

            var axis = Input.GetAxis("Move_Left", "Move_Right");
            if(axis == 0) { return; }

            _playerController2D.ChangeState(new Move(_playerController2D));

        }
        public override void Exit() { }
    }

    public class Move : State
    {
        public override string Name { get { return "Move"; } }
        private const float MOVE_SPEED = 250;

        public Move(PlayerController_2D controller) : base(controller) {}
        public override void Enter()
        {
        
        }
        public override void Execute(double delta)
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
        public override string Name { get { return "Jump"; } }

        private const float BASE_JUMP_SPEED = -500;

        public Jump(PlayerController_2D playerController) : base (playerController) { }
        public override void Enter()
        {
            var velocity = _playerController2D.Velocity;
            velocity.Y = BASE_JUMP_SPEED;
            _playerController2D.Velocity = velocity;

            _playerController2D.ChangeState(_playerController2D.previousState);
        }
        public override void Execute(double delta)
        {

        }
        public override void Exit()
        {
            
        }
    }

    public class VariableJump : State
    {
        public override string Name { get { return "VariableJump"; } }

        private const float BASE_JUMP_SPEED = -500;
        private const float MAX_EXTRA_JUMP_SPEED = -250;
        private float _extraJumpSpeed = -1;

        public VariableJump(PlayerController_2D playerController) : base(playerController) { }
        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("HoldJump");
        }
        public override void Execute(double delta)
        {
            var isJumpLoading = Input.IsActionPressed("Jump");
            var jumpTrigger = Input.IsActionJustReleased("Jump");

            GD.Print(isJumpLoading);
            

            if(isJumpLoading)
            {
                _extraJumpSpeed += (float)(MAX_EXTRA_JUMP_SPEED * delta);
                _extraJumpSpeed = Mathf.Clamp(_extraJumpSpeed, MAX_EXTRA_JUMP_SPEED, 0);

                GD.Print(_extraJumpSpeed);
            }

            if (jumpTrigger)
            {
                var velocity = _playerController2D.Velocity;
                velocity.Y = BASE_JUMP_SPEED + _extraJumpSpeed;
                _playerController2D.Velocity = velocity;

                _playerController2D.ChangeState(_playerController2D.previousState);
            }           
        }
        public override void Exit()
        {

        }
    }

    public class Dash : State
    {
        public override string Name { get { return "Dash"; } }

        private const float DASH_SPEED = 1000;

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

            FreezeGravity();
        }
        public override void Execute(double delta)
        {
            _playerController2D.AnimatedSprite2D.Play("Dash");
        }
        public override void Exit()
        {

        }
        private async void FreezeGravity()
        {
            _playerController2D.gravity = false;
            await _playerController2D.ToSignal(_playerController2D.GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
            _playerController2D.ChangeState(_playerController2D.previousState);
            _playerController2D.gravity = true;
        }
    }


    public abstract class Modificator
    {
        protected PlayerController_2D _playerController2D;

        public Modificator(PlayerController_2D playerController)
        {
            _playerController2D = playerController;
        }
        public abstract void Execute(double delta);
    }

    public class Gravity : Modificator
    {
        private const float GRAVITY = 1500;

        public Gravity(PlayerController_2D playerController) : base(playerController) { }
        public override void Execute(double delta)
        {
            var velocity = _playerController2D.Velocity;

            velocity.Y += (float)delta * GRAVITY;
            _playerController2D.Velocity = velocity;
        }
    }
}
