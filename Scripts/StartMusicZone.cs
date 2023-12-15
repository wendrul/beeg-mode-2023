using Godot;
using System;

public class StartMusicZone : Area2D
{
    private AudioStreamPlayer _music;

    public override void _Ready()
    {
        Connect("body_entered", this, "OnStartMusicZoneBodyEntered");        
        _music = GetNode<AudioStreamPlayer>("./music");
    }

    public void OnStartMusicZoneBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            _music.Play();
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
