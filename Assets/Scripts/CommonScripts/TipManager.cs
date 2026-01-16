using Godot;
using System;
using System.Collections.Generic;

public partial class TipManager : RichTextLabel
{
	
	string[] TipList = 
	{
		"Example Tip"
	};
	
	public override void _Ready()
	{
		// Get random tip and change the text to that
		GD.Randomize();
		Text = TipList[GD.Randi() % TipList.Length];
	}
}
