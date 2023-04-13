using Godot;
using System;

public abstract class State
{
    public abstract string Name { get; }

    protected PlayerController2D _playerController2D;

    public State(PlayerController2D playerController2D)
    {
        _playerController2D = playerController2D;
    }
    public abstract void Enter();
    public abstract void Execute(double delta);
    public abstract void Exit();
}
