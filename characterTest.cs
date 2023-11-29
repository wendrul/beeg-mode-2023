using Godot;
using System;

public partial class characterTest : KinematicBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    public float gravity = 1200f;
    
    private Vector2 Velocity;

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_select") && IsOnFloor()){
            velocity.y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        // Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        float horiz = (Input.IsActionPressed("ui_right") ? 1f:0f) - (Input.IsActionPressed("ui_left") ? 1f:0f);
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
