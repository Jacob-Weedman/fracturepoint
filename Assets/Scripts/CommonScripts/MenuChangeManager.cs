using Godot;
using System;

public partial class MenuChangeManager : Node
{
	public string TargetMenu;
	public float TransitionDelay = 10;
	bool TimerLock = true;
	
	public override void _Ready()
	{
		// Setup timer
		if (TransitionDelay == 10)
		{
			GD.Randomize();
			TransitionDelay += GD.Randf() * 7; // seconds maximum - 10
		}
		// Connect to signal
		GetParent().GetNode<AnimationPlayer>("AnimationPlayer").AnimationFinished += TimerUnlock;
		
		// Stop audio
		GetTree().Root.GetNode<CommonData>("CommonData").StopMusic();
	}
	
	public override void _Process(double delta)
	{ 
		if (TransitionDelay < 0.0f) // Manage waiting
		{
			TransitionDelay = 1;
			TimerLock = true;
			SecondHalf();
		}
		if (TimerLock == false) // Dectiment timer
		{
			TransitionDelay -= (float)delta;
		}
	}
	
	void TimerUnlock(StringName arg)
	{
		if (arg == "LoadIn")
		{
			TimerLock = false;
		}
	}
	
	void SecondHalf()
	{
			//GetTree().Root.GetNode("Camera").GetNode<CameraMovement>("Camera2D").FixedPosition = GetTree().Root.GetNode("Camera").GetNode<Node2D>("Camera2D").Position;
			GetTree().Root.GetNode("Camera").GetNode<CameraMovement>("Camera2D").TeleportCamera(Vector2.Zero);
			
			// Load the target menu
			var scene = GD.Load<PackedScene>($"res://Assets/Scenes/MenusAndLevels/{TargetMenu}.tscn");
			var inst = scene.Instantiate();
			GetTree().Root.AddChild(inst);
			
			// Play audio (AUDIO NAME NEEDS TO BE THE SAME NAME AS THE SCENE)
			if (ResourceLoader.Exists($"res://Assets/Audio/Music/{TargetMenu}.mp3"))
				GetTree().Root.GetNode<CommonData>("CommonData").PlayMusic(TargetMenu);
			
			// Check if the player is there and if it is not then make the camera mode FIXED
			var RootChildren = GetTree().Root.GetChildren();
			if (RootChildren[2].HasNode("Player") == false)
				GetTree().Root.GetNode("Camera").SetMeta("CameraMode", "FIXED");
			
			GetParent().GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation = "LoadOut";
				
			// Check when animation is finished
			GetParent().GetNode<AnimationPlayer>("AnimationPlayer").AnimationFinished += DeleteMenu;
	}
	
	void DeleteMenu(StringName arg)
	{
		if (arg == "LoadOut")
		{
			// Delete change manager
			GetParent().QueueFree();
		}
	}
}
