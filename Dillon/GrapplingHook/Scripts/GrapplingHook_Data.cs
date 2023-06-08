using Godot;

namespace GrapplingHook
{
    public partial class GrapplingHook_Data : Resource
    {
        [ExportCategory("Grappling Hook")]
        [Export]
        public float HookVelocityMultiplikatorOnRelease = 1f;
        [Export]
        public float MaxHookUpwardsVelocityOnRelease = 800f;
        [Export]
        public float PlayerVelocityMultiplikatorOnStart = 1f;
        [Export]
        public float MinDistance = 50f;
        [Export]
        public float MaxDistance = 200f;
        [Export]
        public float StartImpulse = 250f;
        [Export]
        public float MoveSpeed = 700f;
        [Export]
        public float CounterMoveSpeed = 700f;
    }
}
