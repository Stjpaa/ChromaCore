using Godot;
using System;

public partial class PlayerController_2DPhysicBased : RigidBody2D
{
    const float WALK_ACCEL = 500;
    const float WALK_DEACCEL = 500;
    const float WALK_MAX_VELOCITY = 140;
    const float AIR_ACCEL = 100;
    const float AIR_DEACCEL = 100;
    const float JUMP_VELOCITY = 380;
    const float STOP_JUMP_FORCE = 450;
    const float MAX_FLOOR_AIRBORNE_TIME = 0.15f;


    private Vector2 _force = new Vector2(500, 0);
    private Vector2 _forceUp = new Vector2(0, 500);
    private float _torque = 20000;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        var moveRight = Input.IsActionPressed("Move_Right");
        var moveLeft = Input.IsActionPressed("Move_Left");

        var linearVelocity = state.LinearVelocity;
        var step = state.Step;

        if(moveRight)
        {
            GD.Print("up");
            if (linearVelocity.X > -WALK_MAX_VELOCITY)
            {
                linearVelocity.X -= WALK_ACCEL * step;
            }
        }
        else if(moveLeft)
        {
            if(linearVelocity.X < WALK_MAX_VELOCITY)
            {
                linearVelocity.X += WALK_ACCEL * step;
            }
        }

        state.LinearVelocity = linearVelocity;
    }
}
