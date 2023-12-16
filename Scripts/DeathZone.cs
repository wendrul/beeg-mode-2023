using Godot;
using System;
using BeegMode2023.Scripts;

public class DeathZone : Area2D
{
    public override void _Ready()
    {
        Connect("body_entered", this, "OnDeathZoneBodyEnter");        
    }
    
    public void OnDeathZoneBodyEnter(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            Utilities.DeathByWiiGlitch();
        }
    }
    
}
