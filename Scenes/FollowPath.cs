using Godot;
using System;
using BeegMode2023.Scripts;

public class FollowPath : Path2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    [Export] public float speed = 200f;
    public bool HasStarted { get; set; }
    private PathFollow2D _pathFollow2D;
    private PackedScene DeathScreen = GD.Load<PackedScene>("res://Prefabs/BlueScreen.tscn");

    private float t = 0f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      _pathFollow2D =  Utilities.GetChildByType<PathFollow2D>(this, true);
    }

    public override void _Process(float delta)
    {
        if (!HasStarted) return;
        t += delta;
        _pathFollow2D.Offset = t * speed;
    }

    public void On_DestroyWallArea_body_entered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            var screen = DeathScreen.Instance();
            GetChild(0).AddChild(screen);
            HasStarted = true;
        }
    }
}
