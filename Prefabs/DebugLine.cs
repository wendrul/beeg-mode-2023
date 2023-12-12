using Godot;
using System;

public class DebugLine : Line2D
{
    private KinematicBody2D _player;
    [Export] private float durationInSeconds = 3;
    private float t;
    private float pointInterval = 0.01f;
    private int numberOfPoints;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetAsToplevel(true);
        _player = GetNode<KinematicBody2D>("/root/rootNode/CharacterController");
        numberOfPoints = (int)(durationInSeconds / pointInterval);
        ClearPoints();
    }

    public override void _Process(float delta)
    {
        t += delta;
        if (t > pointInterval)
        {
            t = 0;
            AddPoint(_player.Position);
            if (GetPointCount() > numberOfPoints)
            {
                RemovePoint(0);
            }
        }
    }
}
