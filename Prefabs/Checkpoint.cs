using Godot;
using System;
using BeegMode2023.Scripts;

public class Checkpoint : Area2D
{
    [Export] public bool shouldPlayMusicOnrespawn = true;
    public override void _Ready()
    {
        Connect("body_entered", this, "OnCheckpointBodyEnter");        
    }

    public void OnCheckpointBodyEnter(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            GD.Print("Heeeyy");
            Utilities.LastCheckpoint = this.Position;
            Utilities.ShouldPlayMusciOnStart = shouldPlayMusicOnrespawn;
        }
    }
}
