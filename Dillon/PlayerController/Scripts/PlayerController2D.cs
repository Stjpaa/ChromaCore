using Godot;
using PlayerController.States;
using PlayerController.FollowingCamera;
using System;

namespace PlayerController
{
	public partial class PlayerController2D : CharacterBody2D
	{
		// GitHub Commit logs Version 0.0.23

		[Export]
		public PlayerController2D_Data data;
		
		[Export]
		public FollowingCamera.FollowingCamera FollowingCamera;

		[Export]
		public GrapplingHook.GrapplingHook GrapplingHook { get; private set; }

		public AnimatedSprite2D AnimatedSprite2D { get; private set; }

		public Timer DashCooldownTimer { get; private set; }

		public Timer TeleportTimer { get; private set; }

		public Vector2 CheckpointPosition
		{
			get { return _checkpointPosition; }
			private set { _checkpointPosition = value; }
		}

		public bool HookIsReady { get; private set; }

		public State PreviousState { get { return _previousState; } }

		private State _currentState;
		private State _previousState;
		

		private Vector2 _checkpointPosition;

		private Node2D _hookStartPositioNode;

		private Node _parent;

		#region Shader Variables
		private ShaderMaterial _shaderMaterial;
		private Vector2 _startPosition;
		private GpuParticles2D _landingParticle;
		private GpuParticles2D _afterimageRight;
		private GpuParticles2D _afterimageLeft;
		private float _looking_direction;
		#endregion

		#region Balancing
		// Moving state
		public float MaxMoveSpeed { get { return data.maxMoveSpeed; } }
		public float MoveSpeedAcceleration { get { return data.moveSpeedAcceleration; } }
		public float MoveSpeedDecceleration { get { return data.moveSpeedDecceleration; } }

		// Falling state variables
		public float MaxFallSpeed { get { return data.maxFallSpeed; } }
		public float FallSpeedAcceleration { get { return data.fallSpeedAcceleration; } }
		public float MaxFallingMoveSpeed { get { return data.maxFallingMoveSpeed; } }
		public float FallingMoveSpeedAcceleration { get { return data.fallingMoveSpeedAcceleration; } }

		public float FallingMoveSpeedDeceleration { get { return data.FallingMoveSpeedDeceleration; }}

		// Jumping state variables
		public float JumpForce { get { return data.jumpForce; } }
		public float JumpEndModifier { get { return data.jumpEndModifier; } }

		// Dashing state variables
		public float DashSpeed { get { return data.dashSpeed; } }
		public float DashDuration { get { return data.dashDuration; } }
		public float DashCooldown { get { return data.dashCooldown; } }

		// Mechanics variables
		public float CoyoteTimeDuration { get { return data.coyoteTimeDuration; } }
		public float ApexModifierDuration { get { return data.apexModifierDuration; } }
		public float ApexModifierMovementBoost { get { return data.apexModifierMovementBoost; } }

		// Object interaction variables
		public float BoxInteractionImpuls { get { return data.boxInteractionImpuls; } }
		#endregion
		private SoundManager _sound_manager;

		public override void _Ready()
		{
			AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			DashCooldownTimer = GetNode<Timer>("DashCooldown_Timer");
			DashCooldownTimer.WaitTime = DashCooldown;
			TeleportTimer = GetNode<Timer>("Teleport_Timer");
			_hookStartPositioNode = GetNode<Node2D>("HookStartPosition_Node2D");
			_parent = GetParent();

			ChangeState(new Falling(this));

			HookIsReady = true;

			_shaderMaterial = GetNode<CanvasLayer>("CanvasLayer2").GetNode<ColorRect>("ColorRect").Material as ShaderMaterial;
			_startPosition = Transform.Origin;
			_landingParticle = GetNode<GpuParticles2D>("GPUParticles2D");
			_afterimageRight = GetNode<GpuParticles2D>("GPUParticles2D_Afterimage_Right");
			_afterimageLeft = GetNode<GpuParticles2D>("GPUParticles2D_Afterimage_Left");
			_sound_manager = GetNode<SoundManager>("/root/SoundManager");
			FollowingCamera.GlobalPosition = this.GlobalPosition;
			_checkpointPosition = Transform.Origin;
		}

		public override void _Process(double delta)
		{
			_currentState.ExecuteProcess();
			_looking_direction = Input.GetAxis("Move_Left", "Move_Right");
			if(_looking_direction >= 0.1 && Velocity.Length() > 550)
			{
				_afterimageRight.Emitting = true;
				_afterimageLeft.Emitting = false;
			}
			else if(_looking_direction < -0.1 && Velocity.Length() > 550)
			{
				_afterimageLeft.Emitting = true;
				_afterimageRight.Emitting = false;
			}
			else
			{
				_afterimageLeft.Emitting = false;
				_afterimageRight.Emitting = false;
			}
			//GD.Print(Velocity);
		}

		public override void _PhysicsProcess(double delta)
		{
			_currentState.ExecutePhysicsProcess(delta);

			MoveAndSlide();
			CheckCollisionWithBox();

			FollowingCamera.UpdatePosition(GlobalPosition);    
			_shaderMaterial.SetShaderParameter("position", Transform.Origin - _startPosition);       
		}

		public void ChangeState(State newState)
		{
			_previousState = _currentState;
			_currentState = newState;

			_previousState?.Exit();
			_currentState.Enter();

			//GD.Print("Entered state: " + _currentState.Name);
		}

		#region Communication with other game objects
		public bool ShootGrapplingHook()
		{
			if(HookIsReady && GrapplingHook != null)
			{
				GrapplingHook.ShootHook(this);
			}
			return HookIsReady;
		}

		/// <summary>
		/// Called by laser
		/// </summary>
		public void KilledPlayer()
		{
			if (_currentState is Hooking)
			{
				GrapplingHook.ReturnHook();
			}

			if (_currentState is not Dying)
			{
				ChangeState(new Dying(this));
			}      
		}

		/// <summary>
		/// Called by gravity fields
		/// </summary>
		private void ChangeGravityProperties(Vector2 direction)
		{
			if(_currentState is Hooking) 
			{
				GrapplingHook.ReturnHook();
			}
			HookIsReady = false;

			if(_currentState is not Dying)
			{
				GD.Print("Gravity field entered");
				ChangeState(new Falling(this, direction));
			}         
		}

		/// <summary>
		/// Called by gravity fields
		/// </summary>
		private void ResetGravityProperties()
		{
			HookIsReady = true;

			if(_currentState is Dashing) { return; }

			if(_currentState is not Dying)
			{
				GD.Print("Gravity field left");
				ChangeState(new Falling(this));
			}       
		}

		/// <summary>
		/// Called by jump pads
		/// </summary>
		private void ApplyJumpPadForce(Vector2 strength)
		{
			if (_currentState is Hooking)
			{
				GrapplingHook.ReturnHook();
			}

			if (_currentState is Dying)
			{
				return;
			}

			GD.Print("Jumppad entered");
			ChangeState(new Jumping(this, false, strength));
		}

		/// <summary>
		/// Called by portals
		/// </summary>
		private void Teleport(Vector2 newPos, Vector2 impulse)
		{
			if (_currentState is Dying)
			{
				return;
			}

			GD.Print("Teleport entered");
			if (TeleportTimer.TimeLeft > 0) { return; }

			if (_currentState is Hooking)
			{
				GrapplingHook.ReturnHook();
				GrapplingHook.Chain.Visible = false;
			}

			// Set the position to the teleport position
			Transform = new Transform2D(0, newPos);
			ChangeState(new Jumping(this, false, impulse));

			TeleportTimer.Start();
		}

		/// <summary>
		/// Called by traps
		/// </summary>
		private void Respawn()
		{
			if (_currentState is Hooking)
			{
				GrapplingHook.ReturnHook();
			}

			ChangeState(new Dying(this));
		}

		private void CheckCollisionWithBox()
		{
			if (GetSlideCollisionCount() > 0)
			{
				if (GetLastSlideCollision().GetCollider() is Box)
				{
					var @object = GetLastSlideCollision().GetCollider() as Box;
					@object.ApplyCollisionImpulse(new Vector2(BoxInteractionImpuls * Input.GetAxis("Move_Left", "Move_Right"), 0));
				}
			}
		}

		/// <summary>
		/// Called by check points
		/// </summary>
		private void SaveCheckpointLocation(Vector2 checkpointPosition)
		{
			GD.Print("Checkpoint entered");
			_checkpointPosition = checkpointPosition;
		}

		/// <summary>
		/// Called in the dying state when the animation is finished
		/// </summary>
		private void RespawnAtLastCheckpoint()
		{
			var callable = new Callable(this, "RespawnAtLastCheckpoint");

			if (AnimatedSprite2D.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, callable))
			{
				AnimatedSprite2D.Disconnect(AnimatedSprite2D.SignalName.AnimationFinished, callable);
			}
			var offset = new Vector2(0, -22);
			Transform = new Transform2D(0, CheckpointPosition + offset);
			ChangeState(new Falling(this));
		}
		#endregion

		public Vector2 GetHookStartPosition()
		{
			return _hookStartPositioNode.GlobalPosition;
		}

		public void TriggerLandingParticles()
		{
			_landingParticle.Emitting = false;
			_landingParticle.Emitting = true;
			GD.Print("Particle emitted!");
		}

		public void PlaySound(String sound)
		{
			_sound_manager.PlaySound(sound);
		}
	}
}
