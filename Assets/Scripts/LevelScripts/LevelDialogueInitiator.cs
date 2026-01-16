using Godot;
using System;

public partial class LevelDialogueInitiator : Node2D
{
	string[] Dialogue; // Each element in the array is the name of the speaker and the dialogue seperated by a | (ex "Sign|This is a test message.")
	float MaxInteractDistance = 30;
	
	public override void _Ready()
	{
		// Get dialogue from the metadata
		Dialogue = (string[])GetMeta("Dialogue");
	}

	public override void _Process(double delta)
	{
		if (GetParent().GetParent().HasNode("Player"))
		{
			if (Position.DistanceTo(GetParent().GetParent().GetNode<Node2D>("Player").Position) <= MaxInteractDistance)
				GetNode<AnimatedSprite2D>("InteractionIndicator").Visible = true;
			else
				GetNode<AnimatedSprite2D>("InteractionIndicator").Visible = false;
		}
		
		if (Input.IsActionJustPressed("interact") && GetTree().Root.GetNode("Camera").GetNode("Camera2D").HasNode("Dialogue") == false && Position.DistanceTo(GetParent().GetParent().GetNode<Node2D>("Player").Position) <= MaxInteractDistance)
		{
			// Instantiate scene
			var scene = GD.Load<PackedScene>($"res://Assets/Scenes/Dialogue.tscn");
			var inst = scene.Instantiate();
			GetTree().Root.GetNode("Camera").GetNode("Camera2D").AddChild(inst);
			// Send Data
			GetTree().Root.GetNode("Camera").GetNode("Camera2D").GetNode<Dialogue>("Dialogue").SetupDialogue(Dialogue);
		}
	}
}
