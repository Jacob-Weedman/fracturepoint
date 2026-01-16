using Godot;
using System;
using System.Linq;

public partial class PlayerMover : CharacterBody2D
{
	// Movement Variables
	public float Speed = 80.0f;
	public float JumpVelocity = -250.0f;
	
	// Animation Refrences
	AnimatedSprite2D WalkingAnimation;
	
	public override void _Ready()
	{
		// Object refrences
		WalkingAnimation = GetNode<AnimatedSprite2D>("WalkingAnimation");
		// Entering/Exiting various area 2D's
		GetNode<Area2D>("AreaDetector").AreaEntered += AreaEnteredHandler;
		GetNode<Area2D>("AreaDetector").AreaExited += AreaExitedHandler;
		// Change camera
		GetTree().Root.GetNode("Camera").SetMeta("CameraMode", "FOLLOW");
	}

	public override void _PhysicsProcess(double delta)
	{
		
		Vector2 velocity = Velocity;

		// Add gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * 0.8f;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and set velocity
		if (Input.IsActionPressed("move_left")) // move left
		{
			velocity.X = -Speed;
			WalkingAnimation.FlipH = true;
			
			if (WalkingAnimation.IsPlaying() == false) // Play Animation
				WalkingAnimation.Play();
		}
		else if (Input.IsActionPressed("move_right")) // move right
		{
			velocity.X = Speed;
			WalkingAnimation.FlipH = false;
			
			if (WalkingAnimation.IsPlaying() == false) // Play Animation
				WalkingAnimation.Play();
		}
		else
		{
			velocity.X = 0;
			WalkingAnimation.Stop();
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	void AreaEnteredHandler(Area2D arg)
	{
		string name = arg.Name;
		if (name.Contains("SlowField"))
		{
			Speed = 20.0f;
			JumpVelocity = -180.0f;
		}
	}
	void AreaExitedHandler(Area2D arg)
	{
		string name = arg.Name;
		if (name.Contains("SlowField"))
		{
			Speed = 80.0f;
			JumpVelocity = -250.0f;
		}
	}
}
