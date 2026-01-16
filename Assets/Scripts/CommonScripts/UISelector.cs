using Godot;
using System;
using System.Collections.Generic;

public partial class UISelector : Node2D
{
	List<Node2D> AvailableOptions = new List<Node2D>();
	public Node2D SelectedOption;
	int SelectedKey = 0;
	Node2D SelectionIndicator;
	
	public override void _Ready()
	{
		// Set up selection indicator
		SelectionIndicator = GetNode<Node2D>("SelectionIndicator");
		
		// Set up list of options
		foreach (Node2D option in GetChildren())
		{
			if (option != SelectionIndicator)
				AvailableOptions.Add(option);
		}
		
		// Set defaults
		SelectedOption = AvailableOptions[SelectedKey];
		SelectionIndicator.Position = SelectedOption.Position;
	}
	
	public override void _Process(double delta)
	{
		// Select next option
		if(Input.IsActionJustPressed("menu_right") && SelectedKey < AvailableOptions.Count - 1)
		{
			SelectedKey += 1;
		}
		// Select previous option
		if(Input.IsActionJustPressed("menu_left") && SelectedKey != 0)
		{
			SelectedKey -= 1;
			
		}
		
		SelectedOption = AvailableOptions[SelectedKey];
		SelectionIndicator.Position = SelectedOption.Position;
	}
}
