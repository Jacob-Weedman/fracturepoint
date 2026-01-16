using Godot;
using System;

public partial class ChangeMenuInitiator : Node2D
{
	string TargetMenu;
	string TransitionType; // "FAST", "SLOW"
	float TransitionDelay = 0; // seconds
	
	public override void _Ready()
	{
		if (HasMeta("TargetMenu"))
			TargetMenu = (string)GetMeta("TargetMenu");
		if (HasMeta("TransitionType"))
			TransitionType = (string)GetMeta("TransitionType");
		if (HasMeta("TransitionDelay"))
			TransitionDelay = (float)GetMeta("TransitionDelay");
	}

	public override void _Process(double delta)
	{
		// Verify that it is safe and acceptable to change the menu
		if (GetParent().Name == "UISelector")
		{
			if (GetParent<UISelector>().SelectedOption == this && Input.IsActionJustPressed("interact") && GetTree().Root.GetNode("Camera").HasNode("MenuChangeManager") == false)
			{
				ChangeMenu(TransitionType, TargetMenu);
			}
		}
	}
	public void ChangeMenu(string type, string destination)
	{
		if (type == "SLOW")
		{
			// Load the menu change manager
			var scene = GD.Load<PackedScene>("res://Assets/Scenes/MenusAndLevels/MenuChangeManager.tscn");
			var inst = (Node2D)scene.Instantiate();
			GetTree().Root.GetNode("Camera").AddChild(inst);
			
			inst.Position = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").Position;
			
			// Pass args to menu change manager
			inst.GetNode<MenuChangeManager>("MenuChangeManager").TargetMenu = destination;
			inst.GetNode<MenuChangeManager>("MenuChangeManager").TransitionDelay = TransitionDelay;
			
			// Wait for intro animation to finish before freeing the scene
			inst.GetNode<AnimationPlayer>("AnimationPlayer").AnimationFinished += DeleteMenu;
		}
		if (type == "FAST")
		{
			var scene = GD.Load<PackedScene>($"res://Assets/Scenes/MenusAndLevels/{destination}.tscn");
			var inst = (Node2D)scene.Instantiate();
			GetTree().Root.AddChild(inst);
			
			//reset camera
			GetTree().Root.GetNode("Camera").GetNode<CameraMovement>("Camera2D").TeleportCamera(Vector2.Zero);
			GetTree().Root.GetNode("Camera").GetNode<CameraMovement>("Camera2D").FixedPosition = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").Position;
			GetTree().Root.GetNode("Camera").SetMeta("CameraMode", "FIXED");

			DeleteMenu("LoadIn");
		}
		
	}
	void DeleteMenu(StringName arg)
	{
		if (arg == "LoadIn")
		{
			// Delete current scene (MAKE SURE THE PARENT OF THE PARENT OF THIS SCRIPT IS THE SCENE)
			GetParent().GetParent().QueueFree();
		}
	}
	
}
