using Godot;
using System;

namespace GrapplingHook.Physics
{
    public partial class Physics : Node
    {
        public DampedSpringJoint2D Joint { get; private set; }

        public RigidBody2D ChainStart { get; private set; }

        public override void _Ready()
        {
            Joint = GetNode<DampedSpringJoint2D>("DSJ");
            ChainStart = GetNode<RigidBody2D>("DSJ/HookStart");
            Joint.Stiffness = 1000f;
        }

        public void Initialize(Vector2 hookStartPos, Vector2 targetPosition)
        {
            var length =  Mathf.RoundToInt(hookStartPos.DistanceTo(targetPosition));
            SetHookTargetPosition(targetPosition);
            SetHookLength(length);
            SetHookRotation(hookStartPos, targetPosition);
        }

        private void SetHookLength(int length)
        {
            Joint.Length = length;
            Joint.RestLength = length;
            ChainStart.Position = new Vector2(0, length);
        }

        private void SetHookTargetPosition(Vector2 position)
        {
            Joint.GlobalPosition = position;
        }

        private void SetHookRotation(Vector2 startPos, Vector2 endPos)
        {
            var angle = (endPos - startPos).Angle() + (float)Math.PI / 2;
            Joint.Rotation = angle;
        }
    }
}
