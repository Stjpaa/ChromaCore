using Godot;
using System;

namespace GrapplingHook
{
    public partial class HookableArea : Area2D
    {
        public Node2D GrapplingHookRotation
        {
            get;
            private set;
        }
        public Node2D PlayerControllerPosition
        {
            get;
            private set;
        }

        public override void _Ready()
        {
            GrapplingHookRotation = GetNode<Node2D>("GrapplingHookRotation_Node2D");
            PlayerControllerPosition = GrapplingHookRotation.GetNode<Node2D>("PlayerControllerPosition_Node2D");
        }
    }
}
