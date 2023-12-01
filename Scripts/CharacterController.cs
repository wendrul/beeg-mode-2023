using Godot;
using System;

public partial class CharacterController : KinematicBody2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;
    [Export] public float gravity = 1200f;
    
    private Vector2 Velocity;

    public override void _Ready()
    {
        GD.Print("Ready!!");
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor()){
            velocity.y = JumpVelocity;
        }

        float horiz = (Input.IsActionPressed("move_right") ? 1f:0f) 
                        - (Input.IsActionPressed("move_left") ? 1f:0f);
        Vector2 direction = new Vector2(horiz, 0);
        if (direction != Vector2.Zero)
        {
            velocity.x = direction.x * Speed;
        }
        else
        {
            velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
        }

        Velocity = MoveAndSlide(velocity, new Vector2(0, -1));
    }
}
