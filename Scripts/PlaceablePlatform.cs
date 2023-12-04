using Godot;
using System;
using BeegMode2023.Scripts;

public class PlaceablePlatform : RigidBody2D
{
    public bool followMouse = false;
    private Vector2 _mousePos;
    private EditorController _editorController;
    private CollisionShape2D _collider;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _collider = Utilities.GetChildByType<CollisionShape2D>(this, false);
        _collider.Disabled = true;
        _editorController =  Utilities.GetChildByType<EditorController>(GetTree().Root, true);
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
    }


    public override void _Process(float delta)
    {
        _mousePos = GetGlobalMousePosition();
        if (followMouse)
        {
            GlobalPosition = _mousePos;
            if (!Input.IsActionPressed("mouse_click"))
            {
                followMouse = false;
                _collider.Disabled = false;
            }
        }
    }

    private void GetInput()
    {
    }
    

    private void on_StaticBody2D_mouse_entered()
    {
        _editorController.IsHoveringAnObject = true;
    }
   private void On_Floor_mouse_exited() 
   {
        _editorController.IsHoveringAnObject = false;
   }
}
