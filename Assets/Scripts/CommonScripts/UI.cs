using Godot;
using System;

public partial class UI : Node2D
{

	public override void _Process(double delta)
	{
		GlobalPosition = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").GlobalPosition;
	}
}
