using Godot;
using System;

public class Jumping : State
{
    public override string Name { get { return "Jumping"; } }

    private float _jumpForce;

    private bool _applyVariableJumpHeight;


    /// <param name="jumpForce"> If no custom jump force is given, the default jump force from the playercontroller data will be applied </param>
    public Jumping(PlayerController2D playerController_2D, bool applyVariableJumpHeight, float jumpForce = 0) : base(playerController_2D) 
    { 
        _jumpForce = (jumpForce == 0) ? _playerController2D.JumpForce : Mathf.Abs(jumpForce);
        _applyVariableJumpHeight = applyVariableJumpHeight;
    }

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
        // Negavtive velocity means upwards movement
        velocity.Y = -_jumpForce;
        _playerController2D.Velocity = velocity;
    }

    #region Transitions
    private bool CheckFallingTransition()
    {
        if (_playerController2D.IsOnFloor() == false)
        {
            _playerController2D.ChangeState(new Falling(_playerController2D, _applyVariableJumpHeight, false));
            return true;
        }
        return false;
    }
    #endregion
}
