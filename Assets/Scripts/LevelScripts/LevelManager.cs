using Godot;
using System;

// Designed by Jojackman1

public partial class LevelManager : Node
{
	Node2D StartNode;
	Node2D EndNode;
	Node2D Player;
	string EndType = "nothing";
	
	float FreshnessValue;
	
	public override void _Ready()
	{
		StartNode = GetParent().GetNode<Node2D>("StartNode");
		EndNode = GetParent().GetNode<Node2D>("EndNode");
		Player = GetParent().GetNode<Node2D>("Player");
		
		// Play into animation
		GetParent().GetNode("ScreenAnimations").GetNode("LevelStartAssets").GetNode<AnimationPlayer>("LevelStart").CurrentAnimation = "default";
		GetParent().GetNode("ScreenAnimations").GetNode<Node2D>("LevelStartAssets").Reparent(GetTree().Root.GetNode("Camera").GetNode("Camera2D"));
		GetTree().Root.GetNode("Camera").GetNode("Camera2D").GetNode<Node2D>("LevelStartAssets").GlobalPosition = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").GlobalPosition;
		GetTree().Root.GetNode("Camera").GetNode("Camera2D").GetNode<Node2D>("LevelStartAssets").Visible = true;
	}

	public override void _Process(double delta)
	{
		FreshnessValue = GetTree().Root.GetNode<CommonData>("CommonData").FreshnessValue;
		
		// Manage conditions to end the level
		if (EndType == "nothing")
		{
			// Freshness
			if (FreshnessValue <= 0)
			{
				EndManager("Freshness");
				EndType = "Freshness";
			}
			// Success
			if (GetParent().HasNode("Player") && Player.GlobalPosition.DistanceTo(EndNode.GlobalPosition) <= 16)
			{
				EndManager("TemporarySuccess");
				EndType = "TemporarySuccess";
			}
		}
	}
	
	public void EndManager(string EndType)
	{
		switch (EndType)
		{
			case "Freshness": // Pie is no longer fresh
				//Player.QueueFree();
				// Go back to main menu (Replace with proper ending screen and then send back to main menu)
				GetNode<ChangeMenuInitiator>("ChangeMenuInitiator").ChangeMenu("FAST", "MainMenu");
				break;
			case "Mauled": // Crow mauled to death by a fox
				break;
			case "Crushed": // Crow crushed to death by a falling tree
				break;
			case "TemporarySuccess": // Successfully deliver the pie (temporary animation)
					// Remove the player to stop checking
					//Player.QueueFree();
					// Add level completed to the hash set
					GetTree().Root.GetNode<CommonData>("CommonData").LevelsCompleted.Add(GetParent().Name);
					// Play exit animation
					GetParent().GetNode("ScreenAnimations").GetNode("LevelCompleteAssets").GetNode<AnimationPlayer>("LevelComplete").CurrentAnimation = "default";
					GetParent().GetNode("ScreenAnimations").GetNode<Node2D>("LevelCompleteAssets").Reparent(GetTree().Root.GetNode("Camera").GetNode("Camera2D"));
					GetTree().Root.GetNode("Camera").GetNode("Camera2D").GetNode<Node2D>("LevelCompleteAssets").GlobalPosition = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").GlobalPosition;
					GetTree().Root.GetNode("Camera").GetNode("Camera2D").GetNode<Node2D>("LevelCompleteAssets").Visible = true;
					// Go to exit cutscene after exit animation is complete
					var LevelName = (string)GetParent().Name;
					var LevelNumber = LevelName.Split("Level");
					GetNode<ChangeMenuInitiator>("ChangeMenuInitiator").ChangeMenu("SLOW", $"Cutscenes/ExitCutscene{LevelNumber[0]}");
				break;
			case "Success": // Successfully deliver the pie
				break;
			default:
				break;
		}
	}
}
