using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class CameraMovement : Camera2D
{
	// Node Refrence
	Node2D Target;
	Node2D Player;
	
	// Player Info
	Vector2 PreviousPlayerPosition;
	
	// Camera Properties
	float CameraVerticalOffset = -20;
	float CameraHorizontalOffset = 20;
	float CameraMovementSpeed = 4;
	
	// Tweening
	float CameraTweenRate;
	float MaxCameraTweenRate = 150f;
	float MinCameraTweenRate = 0.5f;
	//float MaxCameraDistance = 15f;
	
	// Mode
	public string CameraMode; // "FOLLOW" (Follows player), "FIXED" (Stay at fixed position)
	public Vector2 FixedPosition;
	
	public override void _Ready()
	{
		// Find Nodes
		Target = this.GetParent().GetNode<Node2D>("CameraTarget");
		
		// Set initial camera position
		CameraMode = (string)GetParent().GetMeta("CameraMode");
		Position = Target.Position;
		FixedPosition = new Vector2(0,0);
		PreviousPlayerPosition = new Vector2(0,0);
	}

	
	public override void _Process(double delta)
	{
		// Update Mode
		CameraMode = (string)GetParent().GetMeta("CameraMode");
		// Change Target position depending on current mode
		switch (CameraMode)
		{
			case "FOLLOW":
			// Set the player variable if it equals null
			var RootChildren = GetTree().Root.GetChildren();
			if (Player == null)
			{
				if (RootChildren[2].HasNode("Player"))
					Player = RootChildren[2].GetNode<Node2D>("Player");
			}
			else // If player exists
			{
				// Calculate the player's change in position
				float DeltaPosition = Player.Position[0] - PreviousPlayerPosition.X;
				// Place target in the direction the player is moving for enhanced visibility
				if (DeltaPosition > 0) // Right side of player
				{
					Target.Position = new Vector2(Player.Position[0] + CameraHorizontalOffset, Player.Position[1] + CameraVerticalOffset);
				}
				if (DeltaPosition < 0) // Left side of player
				{
					Target.Position = new Vector2(Player.Position[0] - CameraHorizontalOffset, Player.Position[1] + CameraVerticalOffset);
				}
				if (DeltaPosition == 0) // Center on player when not moving
				{
					Target.Position = new Vector2(Player.Position[0], Player.Position[1] + CameraVerticalOffset);
				}
				PreviousPlayerPosition = Player.Position;
			}
				break;
			case "FIXED":
			// Change position of Camera Target if it is not at the desired fixed position
			if (Target.Position != FixedPosition)
			{
				Target.Position = FixedPosition;
			}
			
			Player = null;
			
				break;
			default: // Does nothing just required
				break;
		}
		// Tween camera to Camera Target's position
			CameraTweenRate = (float)Math.Pow(Position.DistanceTo(Target.Position), 2) / 10f;
			// Ensure the Tween Rate stays between the Max and Min values
			if (CameraTweenRate > MaxCameraTweenRate)
			{
				CameraTweenRate = MaxCameraTweenRate;
			}
			if (CameraTweenRate < MinCameraTweenRate)
			{
				CameraTweenRate = MinCameraTweenRate;
			}
			// Move Camera
			Vector2 MovementDirection = (Target.Position - Position).Normalized();
			Position += MovementDirection * CameraMovementSpeed * CameraTweenRate * (float)delta;
	}
	
	public void TeleportCamera(Vector2 TargetPosition)
	{
		GetParent().GetNode<Node2D>("CameraTarget").Position = TargetPosition;
		Position = TargetPosition;
		FixedPosition = TargetPosition;
	}
	
}
