using Godot;
using System;
using PlayerController;

namespace PlayerController.FollowingCamera
{
    /// <summary>
    /// Camera that follows a target by lerping between the camera position and the target position.
    /// Can be set to different modes which control the following speed of the camrea.
    /// </summary>
    public partial class FollowingCamera : Camera2D
    {
        [Export]
        private PlayerController2D playerNode;
        [Export]
        public float followingSpeedNormal = 30f;
        [Export]
        public float followingSpeedHooking = 150f;

        public CameraModes Mode
        {
            get { return _mode; }
            set 
            { 
                _mode = value; 
                switch (_mode)
                {
                    case CameraModes.Normal:
                        followingSpeed = followingSpeedNormal;
                    break;
                    case CameraModes.Hooking:
                        followingSpeed = followingSpeedHooking;
                    break;
                }
            }
        }

        private CameraModes _mode;

        private float followingSpeed;

        public override void _Ready()
        {
            Mode = CameraModes.Normal;
        }

        public void UpdatePosition(Vector2 targetPosition)
        {
            var target = targetPosition;

            var targetWeightWithoutClamp = 0.9f * (float)GetProcessDeltaTime() * followingSpeed;

            var targetWeight = Mathf.Clamp(targetWeightWithoutClamp, 0, 1);        // Lerp causes Problems when a weigth is >1 -->  Mathf.Lerp(0, 10, 1.5) = Mathf.Lerp(0, 10, 1) + Mathf.Lerp(0, 10, 0.5)
            


            var targePosX = (Mathf.Lerp(GlobalPosition.X, target.X, targetWeight));
            var targePosY = (Mathf.Lerp(GlobalPosition.Y, target.Y, targetWeight));

            

            GlobalPosition = new Vector2(targePosX, targePosY);
        }

        public enum CameraModes
        {
            Normal,
            Hooking,
            Falling
        }
    }
}
