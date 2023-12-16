using Godot;
using System;
using BeegMode2023.Scripts;

public class DeathScreen : CanvasLayer
{
    private bool died;
    private AudioStreamPlayer _wiiDeathSound;

    public override void _Ready()
    {
        _wiiDeathSound = GetNode<AudioStreamPlayer>("./WiiDeathSound");
        Utilities.DeatScreenEntity = this;
        this.Hide();
    }

    public override void _Process(float delta)
    {
        if (this.died && Input.IsActionJustPressed("game_restart"))
        {
            GetTree().ReloadCurrentScene();
        }
    }

    internal void DeathByWiiGlitch()
    {
        Engine.TimeScale = 0;
        Utilities.MusicPlayer.Stop();
        _wiiDeathSound.Play();
        this.died = true;
        this.Show();
    }
}
