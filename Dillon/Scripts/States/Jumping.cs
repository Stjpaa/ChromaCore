using Godot;
using System;

public class Jumping : State
{
    public override string Name { get { return "Jumping"; } }

    public static float JUMP_SPEED { get { return 600f; } }
    public static float JUMP_END_MODIFIER { get { return 10f; } }

    public Jumping(PlayerController_2D playerController_2D) : base(playerController_2D) { }

    public override void Enter() 
    { 
        Jump();
    }

    public override void Execute(double delta)
    {
        if (CheckFallingTransition()) { return; }
    }
    public override void Exit() { }

    private void Jump()
    {
        var velocity = _playerController2D.Velocity;
        velocity.Y -= JUMP_SPEED;
        _playerController2D.Velocity = velocity;

        GD.Print("Jump triggered: Velocity: " + velocity);
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
