using Godot;
using System;

public interface IMechanicMethods 
{
	void ChangeGravityProperties(Vector2 direction);
	void ResetGravityProperties();
	void ApplyJumpPadForce(Vector2 impulse);
	void Teleport(Vector2 newPos, Vector2 impulse);
	void ApplyCollisionImpulse(Vector2 impulse);
	void ResetPosition();
}

public partial class Box : RigidBody2D, IMechanicMethods
{
	#region Physics Variables
	private Vector2 _defaultGravity = new Vector2(0, ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle());
	private Vector2 _currentGravity;
	#endregion

	#region Jump Pad Variables
	private bool _enteredJumpPad = false;

	private Vector2 _jumpPadImpulse;
	#endregion

	#region Portal Variables
	private bool _teleportTriggered = false;

	private Vector2 _teleportPos;

	private Vector2 _teleportImpulse;

	private float _teleportCooldown = 1f;

	private bool _canTeleport = true;

	private Timer _timerNode;

	private bool _collided = false;

	private Vector2 _collisionImpulse;
	#endregion

	#region Reset Variables
	private Vector2 _originalPos;

	private bool _shouldReset = false;

	[Export]
	private bool _canReset = true;
	#endregion

	#region Methods
	public override void _Ready()
	{
		this._currentGravity = this._defaultGravity;
		LinearVelocity = this._currentGravity;
		this._timerNode = GetNode<Timer>("TeleportCooldown");
		this._timerNode.Start(this._teleportCooldown);
		this._originalPos = this.GlobalPosition;

		if (!this._canReset)
		{
			GetNode<CollisionShape2D>("Node2D/ResetArea/CollisionShape2D").Disabled = true;
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		state.LinearVelocity += this._currentGravity * (float)GetPhysicsProcessDeltaTime();
		
		if (this._enteredJumpPad)
		{
			state.ApplyImpulse(this._jumpPadImpulse);
			this._enteredJumpPad = false;
		}

		if (this._teleportTriggered)
		{
			state.Transform = new Transform2D(0, this._teleportPos);
			state.LinearVelocity = new Vector2(0, 0);
			state.ApplyImpulse(this._teleportImpulse);
			this._canTeleport =  false;
			this._teleportTriggered = false;
			this._timerNode.Start(this._teleportCooldown);
		}

		if (this._collided)
		{
			state.ApplyCentralImpulse(this._collisionImpulse);
			this._collided = false;
		}

		if (this._shouldReset)
		{
			GD.Print("Triggered");
			state.Transform = new Transform2D(0, this._originalPos);
			this._shouldReset = false;
		}
	}

	public void ApplyCollisionImpulse(Vector2 impulse)
	{
		this._collided = true;
		this._collisionImpulse = impulse;
	}

	public void ChangeGravityProperties(Vector2 direction)
	{
		this._currentGravity = direction;
	}

	public void ResetGravityProperties()
	{
		this._currentGravity = this._defaultGravity;
	}
	
	public void ApplyJumpPadForce(Vector2 impulse)
	{
		this._enteredJumpPad = true;
		this._jumpPadImpulse = impulse;	
	}

	public void Teleport(Vector2 newPos, Vector2 impulse)
	{
		if (!(this._timerNode.TimeLeft > 0))
		{
			this._teleportPos = newPos;
			this._teleportImpulse = impulse;
			this._teleportTriggered = true;			
		}

	}

	public void ResetPosition()
	{
		this._shouldReset = true;
	}
	#endregion
}
