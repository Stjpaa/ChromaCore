using Godot;
using GrapplingHook.States;
using PlayerController;
using System;
using System.Runtime.Intrinsics.X86;

namespace GrapplingHook
{
    public partial class HookableArea : Area2D
    {
        private Sprite2D _sprite;

        public override void _Draw()
        {
            DrawArc(Vector2.Zero,
                    (GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D).Radius,
                    0, 360, 64, new Color(1, 0, 0));
        }

        public override void _Process(double delta)
        {
            QueueRedraw();
        }

        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            
            BodyEntered += OnPlayerEnteredHookableArea;
            BodyExited += OnPlayerExitedHookableArea;

            _sprite.Visible = false;
            QueueRedraw();
        }

        private void OnPlayerEnteredHookableArea(Node2D body)
        {
            _sprite.Visible = true;
            if(body is PlayerController2D)
            {
                (body as PlayerController2D).GrapplingHook.SetTarget(this);
            }
        }

        private void OnPlayerExitedHookableArea(Node2D body)
        {
            _sprite.Visible = false;
            if (body is PlayerController2D)
            {
                var playerController = body as PlayerController2D;
                if(playerController.GrapplingHook.GetTarget() == this)
                {
                    playerController.GrapplingHook.SetTarget(null);
                }                
            }
        }       
    }
}
