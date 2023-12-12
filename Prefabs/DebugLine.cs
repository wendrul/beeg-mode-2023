using Godot;
using System;

public class DebugLine : Line2D
{
    private KinematicBody2D _player;
    [Export] private int lengthInFrames = 100;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetAsToplevel(true);
       _player = GetNode<KinematicBody2D>("/root/rootNode/CharacterController");
    }

    public override void _Process(float delta)
    {
        AddPoint(_player.Position);
        if (GetPointCount() > lengthInFrames)
        {
            RemovePoint(0);
        }
    }
}
