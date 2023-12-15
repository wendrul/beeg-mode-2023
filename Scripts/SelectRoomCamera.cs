using Godot;
using System;

public class SelectRoomCamera : Area2D
{
    private Camera2D _camera;

    public override void _Ready()
    {
        Connect("body_entered", this, "OnCameraAreaBodyEntered");
        _camera = GetParent<Camera2D>();
    }

    public void OnCameraAreaBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            _camera.MakeCurrent();
        }
    }
}
