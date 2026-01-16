using Godot;
using System;
using System.Collections.Generic;

public partial class LevelSelectionManager : Node2D
{
	Dictionary<string, string> ConnectionRef = new Dictionary<string, string>();
	
	public override void _Ready()
	{
		// Create Dictionary
		ConnectionRef.Add("Level1", "Level2");
		ConnectionRef.Add("Level2", "Level3");
		ConnectionRef.Add("Level3", "Level4");
		ConnectionRef.Add("Level4", "Level5");
		
		// Disable all levels but the first
		foreach (Node2D arg in GetNode("UISelector").GetChildren())
		{
			if (ConnectionRef.ContainsValue(arg.Name))
				arg.SetProcess(false);
		}
		
		// Edit buttons according to what is completed
		foreach (string arg in GetTree().Root.GetNode<CommonData>("CommonData").LevelsCompleted)
		{
			if (ConnectionRef.ContainsKey(arg))
			{
				// Remove fake button
				GetNode("FakeButtons").GetNode<Node2D>(ConnectionRef[arg]).Visible = false;
				// Add real button
				GetNode("UISelector").GetNode<Node2D>(ConnectionRef[arg]).Visible = true;
				GetNode("UISelector").GetNode<Node2D>(ConnectionRef[arg]).SetProcess(true);
			}
		}
	}
}
