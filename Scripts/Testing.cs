using Godot;
using System;
using BeegMode2023.Scripts;

public class Testing : RigidBody2D
{
    [Export] public float speed = 200f;
    public bool followMouse = false;
    private Vector2 velocity;
    private Vector2 mousePos;
    private bool mouseUp;
    private EditorController _editorController;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _editorController =  Utilities.GetChildByType<EditorController>(GetTree().Root, true);
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
    }


    public override void _Process(float delta)
    {
        mousePos = GetGlobalMousePosition();
        if (followMouse)
        {
            GlobalPosition = mousePos;
            if (!Input.IsActionPressed("mouse_click"))
            {
                followMouse = false;
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

