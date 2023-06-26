using Godot;
using PlayerController;

namespace GrapplingHook
{
	/// <summary>
	/// Area to which the grappling hook can be attached.
	/// </summary>
	public partial class HookableArea : Area2D
	{
		[Export]
		public GrapplingHook_Data data;
		[Export]
		public bool showDebugLines;

		private Sprite2D _indicatorIcon;

		public override void _Draw()
		{
			(GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D).Radius = data.MaxDistance;
			var radius = data.MaxDistance;

			// Max distance for grappling hook
			DrawArc(Vector2.Zero,
					radius,
					0, 360, 64, new Color(1, 0, 0), 1, true);

			if (showDebugLines)
			{
				// Min distance for grappling hook
				DrawArc(Vector2.Zero,
						50, 0, 64, 360, new Color(1, 0, 0));

				var dirLeft = Vector2.Right.Rotated(Mathf.DegToRad(160));
				var dirRight = Vector2.Right.Rotated(Mathf.DegToRad(20));
				DrawLine(Vector2.Zero, dirLeft * radius, new Color(0, 1, 0));
				DrawLine(Vector2.Zero, dirRight * radius, new Color(0, 1, 0));

				// Start impuls for grappling hook
				var dirLeft1 = Vector2.Right.Rotated(Mathf.DegToRad(-70));
				var dirRight1 = Vector2.Right.Rotated(Mathf.DegToRad(-110));
				DrawLine(Vector2.Zero, dirLeft1 * radius, new Color(0, 1, 1));
				DrawLine(Vector2.Zero, dirRight1 * radius, new Color(0, 1, 1));
			}
		}

		public override void _Process(double delta)
		{
			QueueRedraw();
		}

		public override void _Ready()
		{
			_indicatorIcon = GetNode<Sprite2D>("Indicator");
			
			BodyEntered += OnPlayerEnteredHookableArea;
			BodyExited += OnPlayerExitedHookableArea;

			_indicatorIcon.Visible = false;
			QueueRedraw();
		}

		private void OnPlayerEnteredHookableArea(Node2D body)
		{           
			if(body is PlayerController2D)
			{
				_indicatorIcon.Visible = true;

				(body as PlayerController2D).GrapplingHook.AddTarget(this);
			}
		}

		private void OnPlayerExitedHookableArea(Node2D body)
		{            
			if (body is PlayerController2D)
			{
				_indicatorIcon.Visible = false;

				var playerController = body as PlayerController2D;
				playerController.GrapplingHook.RemoveTarget(this);               
			}
		}       
	}
}
