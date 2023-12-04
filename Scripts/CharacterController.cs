using Godot;
using System;

public partial class CharacterController : KinematicBody2D
{
    [Export] public float moveSpeed = 300.0f;
    [Export] public float jumpSpeed = -800.0f;
    [Export] public float gravity = 2500f;
    [Export] private float fallSpeedLimit = 1000;

    private const float jumpBufferTime = .08f;
    private float jumpBufferTimer = 0;
    private bool isJumping;
    private const float coyoteTime = .28f;
    private readonly float jumpAscendingGravity = 2400f;
    private float coyoteTimer = 0;
    
    private Vector2 velocity;
    private float peakGravity = 1000f;
    private float velocityAtJumpPeak = 400;
    private float jumpSpeedCut = .2f;
    private bool alreadyCutSpeed;

    public override void _Ready()
    {
        GD.Print("Ready!!");
    }

    public override void _PhysicsProcess(float delta)
    {
        // Vector2 newVelocity = velocity;

        // Add the gravity.

        // Handle Jump.
        Jump(delta);


        float horiz = (Input.IsActionPressed("move_right") ? 1f:0f) 
                        - (Input.IsActionPressed("move_left") ? 1f:0f);
        Vector2 direction = new Vector2(horiz, 0);
        if (direction != Vector2.Zero)
        {
            velocity.x = direction.x * moveSpeed;
        }
        else
        {
            velocity.x = Mathf.MoveToward(velocity.x, 0, moveSpeed);
        }

        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed(), maxVerticalSpeed());
        velocity = MoveAndSlide(velocity, new Vector2(0, -1));
    }

    private void Jump(float delta) 
    {
        coyoteTimer = coyoteTimer > delta ? coyoteTimer - delta : 0;
        jumpBufferTimer = jumpBufferTimer > delta ? jumpBufferTimer - delta : 0;
        if (!IsOnFloor())
        {
            velocity.y += jumpAffectedGravity() * (float)delta;
        }
        if (IsOnFloor()) 
        {
            coyoteTimer = coyoteTime;
            alreadyCutSpeed = false;
        }
        if (Input.IsActionJustPressed("jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        if (Input.IsActionJustReleased("jump") 
                && isJumping && velocity.y < 0 && canMove() && !alreadyCutSpeed)
        {
            alreadyCutSpeed = true;
            velocity.y *= jumpSpeedCut;
        }
        if (coyoteTimer > 0)
        {
            if (jumpBufferTimer > 0)
            {
                velocity.y = jumpSpeed;
                coyoteTimer = 0f;
                jumpBufferTimer = 0f;
                isJumping = true;
            }
        }
    }

    private bool canMove()
    {
        return true;
    }

    private float jumpAffectedGravity()
    {
        if (isJumping && Input.IsActionPressed("jump")
                && velocity.y < 0 && Mathf.Abs(velocity.y) < velocityAtJumpPeak)
        {
            return peakGravity;
        }
        else if (isJumping)
        {
            return jumpAscendingGravity;
        }
        return gravity;
    }

    private float maxVerticalSpeed()
    {
        return fallSpeedLimit;
    }
}