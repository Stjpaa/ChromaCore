using Godot;
using System;

namespace Prototyp
{
    public partial class Player : CharacterBody2D
    {
        public const float MOVE_SPEED = 500;
        public const float GRAVITY = 60;
        public const float MAX_SPEED = 2000;
        public const float FRICTION_AIR = 0.95f;
        public const float FRICTION_GROUND = 0.85f;
        public const float CHAIN_PULL = 105;

        public Vector2 playerVelocity;
        public Vector2 chainVelocity;

        public Chain chain;

        public override void _Ready()
        {
            chain = GetParent().GetNode<Chain>("Chain/Chain");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton)
            {
                if (@event.IsPressed())
                {
                    chain.Shoot(GetGlobalMousePosition(), this);
                }
                else
                {
                    chain.Release();
                }
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            playerVelocity = Velocity;

            var walk = ((Input.GetActionStrength("Move_Right") - Input.GetActionStrength("Move_Left")) * MOVE_SPEED);

            playerVelocity.Y += GRAVITY;

            if (chain.hooked)
            {
                chainVelocity = ToLocal(chain.tipTargetPosition - GlobalPosition).Normalized() * CHAIN_PULL;

                if (Mathf.Sign(chainVelocity.X) != Mathf.Sign(walk))
                {
                    chainVelocity.X *= 0.7f;
                }
            }
            else
            {
                chainVelocity = Vector2.Zero;
            }

            playerVelocity += chainVelocity;

            playerVelocity.X += walk;

            Velocity = playerVelocity;

            MoveAndSlide();

            playerVelocity.X -= walk;

            playerVelocity.Y = Mathf.Clamp(playerVelocity.Y, -MAX_SPEED, MAX_SPEED);
            playerVelocity.X = Math.Clamp(playerVelocity.X, -MAX_SPEED, MAX_SPEED);
            var grounded = IsOnFloor();

            if (grounded)
            {
                playerVelocity.X *= FRICTION_GROUND;
                if (playerVelocity.Y >= 5)
                {
                    playerVelocity.Y = 5;
                }
            }
            else if (IsOnCeiling() && playerVelocity.Y <= -5)
            {
                playerVelocity.Y = -5;
            }

            if (grounded == false)
            {
                playerVelocity.X *= FRICTION_AIR;
                if (playerVelocity.Y > 0)
                {
                    playerVelocity.Y *= FRICTION_AIR;
                }
            }

            Velocity = playerVelocity;
            GD.Print(Velocity);
        }

    }
}
