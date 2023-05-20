using Godot;
using System;

namespace GrapplingHook.Physics
{
    /// <summary>
    /// Contains the physisc for the grappling hook.
    /// <para>
    /// Implemented as a dampedspringjoint. Bending is avoided by setting its stiffness to a high value.
    /// It has two childs which are assigned to the dsj.
    /// A staticbody2d which is the hook end and a rigidbody2d which is the hook start.
    /// Due to the node composition only the dsj has to be set to the target position and then rotated towards the players hook start position.
    /// Then the length of the dsj is set and the local position of the hook start node is also set to the length.
    /// By applying forces to the hook start a swinging mechanic is simulated.
    /// </para>
    /// </summary>
    public partial class Physics : Node
    {
        public DampedSpringJoint2D Joint { get; private set; }

        public HookStart HookStart { get; private set; }

        public override void _Ready()
        {
            Joint = GetNode<DampedSpringJoint2D>("DSJ");
            HookStart = GetNode<HookStart>("DSJ/HookStart");
            // To prevent bending of the spring
            Joint.Stiffness = 2000f;
        }

        public void Initialize(Vector2 hookStartPos, Vector2 targetPosition)
        {
            var length =  Mathf.RoundToInt(hookStartPos.DistanceTo(targetPosition));
            SetHookTargetPosition(targetPosition, hookStartPos);
            SetHookLength(length);
            SetHookRotation(hookStartPos, targetPosition);
        }

        private void SetHookLength(int length)
        {
            Joint.Length = length;
            Joint.RestLength = length;
            HookStart.Position = new Vector2(0, length);
        }

        private void SetHookTargetPosition(Vector2 targetPosition, Vector2 hookStartPosition)
        {
            Joint.GlobalPosition = targetPosition;
            HookStart.SetPosition(hookStartPosition);
        }

        private void SetHookRotation(Vector2 startPos, Vector2 endPos)
        {
            var angle = (endPos - startPos).Angle() + (float)Math.PI / 2;
            Joint.Rotation = angle;
        }
    }
}
