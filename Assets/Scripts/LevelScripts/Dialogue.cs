using Godot;
using System;
using System.Collections.Generic;

// Instantiate this scene and send all data to SetupDailogue()
// arg contains the speaker and the sentance for example "Sign|This is a test."

public partial class Dialogue : Node2D
{
	string SpeakerName = null;
	string SpeakerImage = null;
	string Sentance = null;
	Queue<string> Data = new Queue<string>();
	
	float ScrollTimer = 0.05f; // Sec
	float ScrollTimerReset = 0.05f;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact")) // Advance dialogue
		{
			AdvanceDialogue();
		}
		
		// Scroll text
		ScrollTimer -= (float)delta;
		if (Sentance != null && ScrollTimer <= 0)
		{
			ScrollTimer = ScrollTimerReset;
			GetNode<RichTextLabel>("Dialogue").VisibleCharacters += 1;
		}
	}

	public void SetupDialogue(string[] arg)
	{
		GetNode<RichTextLabel>("Dialogue").VisibleCharacters = 0;
		
		// Set up data
		foreach (string value in arg)
		{
			Data.Enqueue(value);
		}
		
		// Set up visuals & first dialogue option
		if (Data.Count > 0)
		{
			// Get data
			string[] OperatingData = Data.Dequeue().Split("|");
			SpeakerName = OperatingData[0];
			SpeakerImage = OperatingData[1];
			Sentance = OperatingData[2];
			//Display data
			GetNode<Sprite2D>("Icon").Texture = (Godot.Texture2D)GD.Load($"res://Assets/Sprites/DialogueIcons/{SpeakerImage}.png");
			GetNode<RichTextLabel>("SpeakerName").Text = SpeakerName;
			GetNode<RichTextLabel>("Dialogue").Text = Sentance;
		}
		else // If there is nothing left delete the scene
		{
			QueueFree();
		}
	}
	
	public void AdvanceDialogue()
	{
		// Determine wether to skip to the end or display the next sentance
		if (GetNode<RichTextLabel>("Dialogue").GetTotalCharacterCount() > GetNode<RichTextLabel>("Dialogue").VisibleCharacters)
		{
			GetNode<RichTextLabel>("Dialogue").VisibleCharacters = GetNode<RichTextLabel>("Dialogue").GetTotalCharacterCount();
		}
		else
		{
			if (Data.Count > 0)
			{
					GetNode<RichTextLabel>("Dialogue").VisibleCharacters = 0;
					// Get data
					string[] OperatingData = Data.Dequeue().Split("|");
					SpeakerName = OperatingData[0];
					SpeakerImage = OperatingData[1];
					Sentance = OperatingData[2];
					//Display data
					GetNode<Sprite2D>("Icon").Texture = (Godot.Texture2D)GD.Load($"res://Assets/Sprites/DialogueIcons/{SpeakerImage}.png");
					GetNode<RichTextLabel>("SpeakerName").Text = SpeakerName;
					GetNode<RichTextLabel>("Dialogue").Text = Sentance;
			}
			else // If there is nothing left delete the scene
			{
				QueueFree();
			}
		}
	}
}
