using Godot;
using System;
using BeegMode2023.Scripts;

public class music : AudioStreamPlayer
{
    public override void _Ready()
    {
       Utilities.MusicPlayer = this; 
    }
}
