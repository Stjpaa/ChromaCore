using Godot;
using GrapplingHook;


namespace PlayerController.States
{
    /// <summary>
    /// This state is used when the player is using the grappling hook.
    /// More like a pseudo state becouse the grappling hook implements the controls and the mechanics for the hooking.
    /// 
    /// <para> Transition to jumping --> The player released the grappling hook. </para>
    /// </summary>
    public partial class Hooking : State
    {
        public override string Name { get { return "Hooking"; } }

        private GrapplingHook.GrapplingHook _grapplingHook;

        private uint _collisionMask;

        public Hooking(PlayerController2D playerController2D) : base(playerController2D) 
        {
            _grapplingHook = _playerController2D.GrapplingHook;
            _collisionMask = _playerController2D.CollisionMask;
        }

        public override void Enter()
        {
            _playerController2D.AnimatedSprite2D.Play("Hooking");
            _playerController2D.Velocity = Vector2.Zero;
            _grapplingHook.ReleaseEvent += TransitionToJumping;
            
            DeactivateCollisionDetection();
            _playerController2D.FollowingCamera.Mode = FollowingCamera.FollowingCamera.CameraModes.Hooking;
        }

        public override void Exit() 
        { 
            ActivateCollisionDetection();
        }

        private void ActivateCollisionDetection()
        {
            _playerController2D.CollisionMask = _collisionMask;
        }

        private void DeactivateCollisionDetection()
        {
            _playerController2D.CollisionMask = 0;
        }

        private void TransitionToJumping()
        {
            _grapplingHook.ReleaseEvent -= TransitionToJumping;
            _playerController2D.ChangeState(new Jumping(_playerController2D, false, _grapplingHook.GetHookVelocityOnRelease()));          
        }
    }
}
