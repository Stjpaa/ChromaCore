using Godot;
using System;

public partial class Hooking : State
{
    public override string Name { get { return "Hooking"; } }
    public Hooking(PlayerController2D playerController2D) : base(playerController2D)
    {

    }

    public override void Enter()
    {
        var mousePosition = _playerController2D.GetGlobalMousePosition();
        _playerController2D.GrapplingHook.SetHookPosition(mousePosition);
    }

    public override void Execute(double delta)
    {
        _playerController2D.ChangeState(_playerController2D.previousState);
    }

    public override void Exit()
    {

    }
}
