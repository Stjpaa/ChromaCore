using Godot;
using GrapplingHook.States;
using PlayerController;
using System;

namespace GrapplingHook
{
    public partial class HookableArea : Area2D
    {
        private Sprite2D _sprite;

        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            
            BodyEntered += OnPlayerEnteredHookableArea;
            BodyExited += OnPlayerExitedHookableArea;

            _sprite.Visible = false;
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
                (body as PlayerController2D).GrapplingHook.SetTarget(this);
            }
        }       
    }
}
