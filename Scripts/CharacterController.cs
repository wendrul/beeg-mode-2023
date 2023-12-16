using Godot;
using System;
using BeegMode2023.Scripts;

public partial class CharacterController : KinematicBody2D
{
    public float moveSpeed = 300.0f;//350
    private float maxMoveSpeed = 300.0f;
    public float jumpSpeed = -550f;//650
    public float gravity = 1250f;
    private float fallSpeedLimit = 800f;

    private const float jumpBufferTime = .08f;
    private float jumpBufferTimer = 0;
    private bool canWallJump = true;
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
    private RayCast2D _leftWallCheck;
    private RayCast2D _rightWallCheck;
    private float wallHugFallSpeed = 80f;
    private bool isHuggingWall;
    private float wallJumpBufferTimer;
    private float wallJumpLoseControlTimer = 0f;
    private float wallJumpLoseControlTime = 0.2f;
    private float lastWallDir = 1;
    private float wallJumpCoyoteTime = .08f;
    private float wallJumpCoyoteTimer = 0f;
    private float stickToWallTime = .08f;
    private float stickToWallTimer = 0f;
    private float wallJumpHorizontalSpeed = 500f;
    private float wallJumpVerticalSpeed = 500f;
    private float horizAccel = 20f;
    private float horizFriction = 50f;
    private float minSpeed= 1f;

    public bool EditorModeLockInputs { get; set; } = false;

    public override void _Ready()
    {
        this.Position = Utilities.LastCheckpoint;
        GD.Print("Ready!!");
        moveSpeed = maxMoveSpeed;
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _leftWallCheck = GetNode<RayCast2D>("leftWallCheck");
        _rightWallCheck = GetNode<RayCast2D>("rightWallCheck");
    }


    public override void _Process(float delta)
    {
        AnimationLogic();
    }

    public void AnimationLogic()
    {
        if (!IsOnFloor())
        {
            if (_leftWallCheck.IsColliding())
            {
                _animatedSprite.Play("WallSlide");
                _animatedSprite.FlipH = true;
            }
            else if (_rightWallCheck.IsColliding())
            {
                _animatedSprite.Play("WallSlide");
                _animatedSprite.FlipH = false;
            }
            else
            {
                _animatedSprite.Play("Idle");
            }
        }
        else if (Mathf.Abs(velocity.x) > 0.01 && !isJumping) {
            _animatedSprite.FlipH = velocity.x < 0;
            _animatedSprite.Play("Run");
        }
        else {
            _animatedSprite.Play("Idle");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity, new Vector2(0, -1));
        if (IsOnFloor() || IsOnWall() || IsOnCeiling())
        {
            wallJumpLoseControlTimer = 0;
        }
        Jump(delta);
        WallHug(delta);
        WallJump(delta);
        if (velocity.y > 0)
        {
            isJumping = false;
        }
        HorizontalMovement(delta);


        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed(), maxVerticalSpeed());
    }

    private bool canMove()
    {
        return !EditorModeLockInputs && wallJumpLoseControlTimer <= 0.00001;
    }

    private void HorizontalMovement(float delta)
    {
        float horiz = (Input.IsActionPressed("move_right") ? 1f:0f)
                        - (Input.IsActionPressed("move_left") ? 1f:0f);

        if (canMove())
        {
            if (horiz != 0)
            {
                velocity.x = Mathf.Lerp(velocity.x, horiz * moveSpeed, horizAccel * delta);
            }
            if (Mathf.Abs(horiz) <= 0.0001)
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, horizFriction * delta);
                velocity.x = Mathf.Abs(velocity.x) < minSpeed ? 0 : velocity.x;
            }
        }
    }

    private void WallHug(float delta)
    {
        stickToWallTimer = stickToWallTimer > delta ? stickToWallTimer - delta : 0;
        if (!IsOnFloor() && (_leftWallCheck.IsColliding() || _rightWallCheck.IsColliding()))
        {
            lastWallDir = _leftWallCheck.IsColliding() ? 1 : -1;
            isHuggingWall = true;
            alreadyCutSpeed = false;
            float horiz = (Input.IsActionPressed("move_right") ? 1f:0f)
                        - (Input.IsActionPressed("move_left") ? 1f:0f);
            if ((horiz < 0 && _leftWallCheck.IsColliding())
                    || (horiz > 0 && _rightWallCheck.IsColliding()))
            {
                stickToWallTimer = stickToWallTime;
            }
            if (stickToWallTimer > 0)
            {
                velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed(), wallHugFallSpeed);
            }
            return ;
        }
        isHuggingWall = false;
    }

    private void WallJump(float delta)
    {
        wallJumpBufferTimer = wallJumpBufferTimer > delta ? wallJumpBufferTimer - delta : 0;
        wallJumpCoyoteTimer = wallJumpCoyoteTimer > delta ? wallJumpCoyoteTimer - delta : 0;
        wallJumpLoseControlTimer = wallJumpLoseControlTimer > delta ?
                wallJumpLoseControlTimer - delta : 0;
        if (Input.IsActionJustPressed("jump") && canWallJump)
        {
            wallJumpBufferTimer = jumpBufferTime;
        }
        if (isHuggingWall)
        {
            wallJumpCoyoteTimer = wallJumpCoyoteTime;
        }
        if (wallJumpCoyoteTimer > 0)
        {
            if (wallJumpBufferTimer > 0 && !EditorModeLockInputs)
            {
                isJumping = true;
                velocity.y = -wallJumpVerticalSpeed;
                velocity.x = lastWallDir * wallJumpHorizontalSpeed;
                wallJumpBufferTimer = 0;
                wallJumpLoseControlTimer = wallJumpLoseControlTime;
            }
        }
    }

    private void Jump(float delta)
    {
        coyoteTimer = coyoteTimer > delta ? coyoteTimer - delta : 0;
        jumpBufferTimer = jumpBufferTimer > delta ? jumpBufferTimer - delta : 0;
        velocity.y += jumpAffectedGravity() * (float)delta;
        if (IsOnFloor())
        {
            coyoteTimer = coyoteTime;
            alreadyCutSpeed = false;
        }
        if (Input.IsActionJustPressed("jump") || Input.IsActionJustPressed("next_dialog"))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        canWallJump = true;
        if ((Input.IsActionJustReleased("jump") || Input.IsActionJustReleased("next_dialog"))
                && isJumping && velocity.y < 0 && canMove() && !alreadyCutSpeed)
        {
            alreadyCutSpeed = true;
            velocity.y *= jumpSpeedCut;
        }
        if (coyoteTimer > 0)
        {
            if (jumpBufferTimer > 0 && !EditorModeLockInputs)
            {
                canWallJump = false;
                velocity.y = jumpSpeed;
                coyoteTimer = 0f;
                jumpBufferTimer = 0f;
                isJumping = true;
            }
        }
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

    internal void StopToListen()
    {
        moveSpeed = 0;
        velocity.x = 0;
        _animatedSprite.Stop();
        _animatedSprite.Play("Idle");
    }

    internal void ResumeHorizontalMovement()
    {
        moveSpeed = maxMoveSpeed;
    }
}
