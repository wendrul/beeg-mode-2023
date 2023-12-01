using Godot;
using System;

public class Testing : RigidBody2D
{
    [Export] public float speed = 200f;
    private bool followMouse = false;
    private Vector2 velocity;
    private Vector2 mousePos;
    private bool mouseUp;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
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
                GD.Print("Mouse Up");
                GD.Print("Pos after: " +GlobalPosition);

                followMouse = false;
            }


        }
    }

    private void GetInput()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


    private void on_StaticBody2D_mouse_entered()
    {
        GD.Print("Mouse: enter ");

    }

    private void On_StaticBody2D_input_event(Node2D Viewport, InputEvent inputEvent, int shape_idx)
    {
        if (inputEvent is InputEventMouseButton eventMouseButton  )
        {
            if (eventMouseButton.IsPressed())
            {
                GD.Print("Pos before: " +GlobalPosition);
                GD.Print("Mouse down");
                followMouse = true;
            }
       
        }
      
    }
}

