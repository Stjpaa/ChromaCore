using Godot;
using System;
using System.Collections.Generic;

public partial class LevelRotator : Node
{
	[Export]
	private Node2D _player;
	[Export(PropertyHint.Range, "0,180,1")]
	private float speed = 30;
	[Export]
	private Godot.Collections.Array<NodePath> _rotatingLists;
	[Export]
	private Godot.Collections.Array<NodePath> _nonRotatingLists;

	private Node2D[] rotating_nodes;
	private Node2D[] non_rotating_nodes;
	private float angle = 0;
	private bool rotating = false;
	public override void _Ready()
	{
		{
			List<Node2D> rotating_nodes_list = new List<Node2D>();

			for(int i = 0; i < _rotatingLists.Count; i++)
			{
				Godot.Collections.Array<Godot.Node> children = ((Node)GetNode(_rotatingLists[i])).GetChildren();
				for(int j = 0; j < children.Count; j++)
				{
					try
					{
						Node2D node = (Node2D)children[j];
						rotating_nodes_list.Add(node);
					}
					catch(InvalidCastException)
					{ /* do nothing */ }
				}
			}

			rotating_nodes = rotating_nodes_list.ToArray();
		}

		{
			List<Node2D> non_rotating_nodes_list = new List<Node2D>();

			for(int i = 0; i < _nonRotatingLists.Count; i++)
			{
				Godot.Collections.Array<Godot.Node> children = ((Node)GetNode(_nonRotatingLists[i])).GetChildren();
				for(int j = 0; j < children.Count; j++)
				{
					try
					{
						Node2D node = (Node2D)children[j];
						non_rotating_nodes_list.Add(node);
					}
					catch(InvalidCastException)
					{ /* do nothing */ }
				}
			}

			non_rotating_nodes = non_rotating_nodes_list.ToArray();
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("rotate_level"))
		{
			rotating = !rotating ? true : rotating;
		}

		if(rotating)
		{
			float delta_f = ((float)delta)*speed;
			angle += delta_f;
			
			Rotate(delta_f);

			if(angle >= 90)
			{
				Rotate(90-angle);
				rotating = false;
				angle = 0;
			}
		}
	}

	public void Rotate()
	{
		rotating = !rotating ? true : rotating;
	}

	private void Rotate(float angle)
	{
		Vector2 player_position = _player.GetGlobalTransform().Origin;

		for(int i = 0; i < rotating_nodes.Length; i++)
		{
			Node2D node = rotating_nodes[i];
			Vector2 node_position = node.GetGlobalTransform().Origin;
			Vector2 offset = node_position - player_position;
			offset = offset.Rotated(angle*Mathf.Pi/180);
			node.Rotation += angle*Mathf.Pi/180;
			node.Position = player_position + offset;
		}

		for(int i = 0; i < non_rotating_nodes.Length; i++)
		{
			Node2D node = non_rotating_nodes[i];
			Vector2 node_position = node.GetGlobalTransform().Origin;
			Vector2 offset = node_position - player_position;
			offset = offset.Rotated(angle*Mathf.Pi/180);
			node.Position = player_position + offset;
		}
	}
}
