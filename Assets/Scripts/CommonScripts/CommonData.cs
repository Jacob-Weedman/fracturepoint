using Godot;
using System;
using System.Collections.Generic;

public partial class CommonData : Node
{
	public bool Muted = true;
	
	// Level information
	public string CurrentLevel;
	public int CurrentBerryCount;
	public float FreshnessValue;
	
	// Level high scores
	public int Level1HighScore = 0;
	public int Level2HighScore = 0;
	public int Level3HighScore = 0;
	public int Level4HighScore = 0;
	public int Level5HighScore = 0;
	
	// Levels Completed
	public HashSet<string> LevelsCompleted = new HashSet<string>();
	
	public override void _Ready()
	{
		// Connect to the audio player
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Finished += LoopAudio;
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").VolumeDb = -80; // temporary
		
		// Start game by loading the camera
		var scene = GD.Load<PackedScene>("res://Assets/Scenes/Persistant/Camera.tscn");
		var inst = scene.Instantiate();
		GetTree().Root.CallDeferred(Node.MethodName.AddChild, inst);
		
		// Start game by loading the main menu
		scene = GD.Load<PackedScene>("res://Assets/Scenes/MenusAndLevels/MainMenu.tscn");
		inst = scene.Instantiate();
		GetTree().Root.CallDeferred(Node.MethodName.AddChild, inst);
		PlayMusic("MainMenu");
		
		// Load in saved data
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("quit_game"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("mute"))
		{
			if (Muted)
			{
				Muted = false;
				GetNode<AudioStreamPlayer>("AudioStreamPlayer").VolumeDb = 0;
			}
			else
			{
				Muted = true;
				GetNode<AudioStreamPlayer>("AudioStreamPlayer").VolumeDb = -80;
			}
		}
	}
	
	// BG Audio Management
	public void StopMusic()
	{
		// Stop whatever audio is playing
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
	}
	public void PlayMusic(string AudioName)
	{
		// Load target audio and play the audio
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = (Godot.AudioStream)GD.Load($"res://Assets/Audio/Music/{AudioName}.mp3");
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}
	void LoopAudio()
	{
		// Find when the stream finishes playing to play it again
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}
}
