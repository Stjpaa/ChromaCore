using Godot;
using System;

namespace Prototyp
{
    public partial class Chain : Node2D
    {
        public Line2D links;
        public CharacterBody2D tip;

        public Vector2 direction;

        public Vector2 tipTargetPosition;
        public Player player;

        public const float SPEED = 1500f;

        public bool flying = false;
        public bool hooked = true;

        public override void _Ready()
        {
            links = GetNode<Line2D>("Links");
            tip = GetNode<CharacterBody2D>("Tip");
        }

        public override void _Process(double delta)
        {
            Visible = flying || hooked;
            if (Visible == false || player == null)
            {
                return;
            }

            links.GlobalPosition = tip.GlobalPosition;
            var offset = tipTargetPosition.DistanceTo(player.GlobalPosition);
            links.SetPointPosition(1, new Vector2(offset, 0));

            links.LookAt(player.GlobalPosition);
        }

        public override void _PhysicsProcess(double delta)
        {
            if (flying)
            {
                var collision = tip.MoveAndCollide(direction * SPEED * (float)delta);

                if (collision != null)
                {
                    hooked = true;
                    flying = false;
                }
            }
        }

        public void Shoot(Vector2 target, Player p)
        {
            player = p;
            tipTargetPosition = target;
            direction = (target - p.GlobalPosition).Normalized();
            flying = true;

            GlobalPosition = p.GlobalPosition;
            LookAt(target);
            tip.GlobalPosition = p.GlobalPosition;
        }

        public void Release()
        {
            flying = false;
            hooked = false;
        }
    }
}
