using Godot;
using System;

public partial class CharacterController : KinematicBody2D
{
    public float moveSpeed = 300.0f;//350
    public float jumpSpeed = -550f;//650
    public float gravity = 1250f;
    private float fallSpeedLimit = 800f;

    private const float jumpBufferTime = .08f;
    private float jumpBufferTimer = 0;
    private bool isJumping;
    
    private const float coyoteTime = .08f;
    private float coyoteTimer = 0;
   
    private readonly float jumpAscendingGravity = 2500f;
    
    private Vector2 velocity;
    private float peakGravity = 500f;
    private float velocityAtJumpPeak = 200;
    private float jumpSpeedCut = .2f;
    private bool alreadyCutSpeed;
    private AnimatedSprite _animatedSprite;

    public bool EditorModeLockInputs { get; set; } = false;

    public override void _Ready()
    {
        GD.Print("Ready!!");
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _Process(float delta)
    {
        AnimationLogic();
    }

    public void AnimationLogic()
    {
        if (Mathf.Abs(velocity.x) > 0.01 && !isJumping) {
            _animatedSprite.FlipH = velocity.x < 0;
            _animatedSprite.Play("Run");
        }
        else {
            _animatedSprite.Play("Idle");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Jump(delta);
        if (velocity.y > 0)
        {
            isJumping = false;
        }
            

        float horiz = (Input.IsActionPressed("move_right") ? 1f:0f) 
                        - (Input.IsActionPressed("move_left") ? 1f:0f);
        Vector2 direction = new Vector2(horiz, 0);
        if (!EditorModeLockInputs)
        {
            if (direction != Vector2.Zero)
            {
                velocity.x = direction.x * moveSpeed;
            }
            else
            {
                velocity.x = Mathf.MoveToward(velocity.x, 0, moveSpeed);
            }
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
                && isJumping && velocity.y < 0 && canMove() && !alreadyCutSpeed && !EditorModeLockInputs)
        {
            alreadyCutSpeed = true;
            velocity.y *= jumpSpeedCut;
        }
        if (coyoteTimer > 0)
        {
            if (jumpBufferTimer > 0 && !EditorModeLockInputs)
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
        if (isJumping && (Input.IsActionPressed("jump") || EditorModeLockInputs)
                && velocity.y < 0 && Mathf.Abs(velocity.y) < velocityAtJumpPeak && !alreadyCutSpeed)
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
