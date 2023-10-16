using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    private Vector2 input;

    // state
    private bool isFacingRight;
    private bool isJumping;

    private bool isWallJumping;


    // timers

    private float lastGrounded;
    private float lastOnWall;
	private float lastOnWallRight;
	private float lastOnWallLeft;
    private float lastPressedJump;


    // Jumping
    private bool isFinishedJumping;
    private bool isFallingAfterJump;
    [SerializeField] public int bonusJumps = 1;

    [SerializeField] private float wallJumpTime;

    // Collision checks
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform frontChck;
	[SerializeField] private Transform backCheck;


    // movement parameters
    private float gravityScale;
    private float gravityStrength;
    private float jumpingForce;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15, 25);

    private int wallJumpDirection;

    [SerializeField] float fallGravityMultiplier = 1.5f;
    [SerializeField] float fallGravityMultiplierAfterJump = 2f;
    [SerializeField] float maxFallingVelocity = 25f;
    [SerializeField] public float jumpHeight = 3.5f;
    [SerializeField] public float jumpApexTime = 0.3f;

    [SerializeField] public float jumpHangTimeThreshold = 1f;
    [SerializeField] public float jumpHangAccelerationMultiplier = 1.1f;
    [SerializeField] public float jumpHangMaxVelocityMultiplier = 1.3f;

    [SerializeField] public float maxVelocity = 10f;
    [SerializeField] public float runningAcceleration = 2.5f;
    [SerializeField] public float runningDeceleration = 5f;

    private float trueAcceleration;
    private float trueDeceleration;
    [SerializeField] public float airFriction = 0.65f;

    [SerializeField] public float coyoteTime = 0.1f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetMovementParams();
        rb.gravityScale = gravityScale;
        isFacingRight = true;
    }

    // Update is called once per frame

    void UpdateTimers()
    {
        lastGrounded-= Time.deltaTime;
        lastOnWall-= Time.deltaTime;
        lastOnWallLeft-= Time.deltaTime;
        lastOnWallRight -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;
    }

    void HandleInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

        // if moving on the x axis
        if (input.x != 0)
        {
            // if moving right and is not facing right, flip
            if (input.x > 0 != isFacingRight)
            {
			    Flip();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            lastPressedJump = 0.1f;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (isJumping && rb.velocity.y > 0)
            {
                isFinishedJumping = true;
            }
        }

    }

    bool isCollidingWithGround()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0, LayerMask.GetMask("Ground"));
    }

    bool isCollidingWithWall(bool isRight)
    {
        return (Physics2D.OverlapBox(frontChck.position, frontChck.localScale, 0, LayerMask.GetMask("StickyWall")) && isRight) || (Physics2D.OverlapBox(backCheck.position, backCheck.localScale, 0, LayerMask.GetMask("StickyWall")) && !isRight);
    }

    void UpdateCollisions()
    {
        // Grounded
        if (isCollidingWithGround())
        {
            lastGrounded = coyoteTime;
            bonusJumps = 1;
        }

        // if we are on a wall to the right
        if (isCollidingWithWall(isFacingRight))
        {
            lastOnWallRight = coyoteTime;
        }

        // if we are on a wall to the left
        if (isCollidingWithWall(!isFacingRight))
        {
            lastOnWallLeft = coyoteTime;
        }

        lastOnWall = Mathf.Max(lastOnWallLeft, lastOnWallRight);
    }

    void UpdateGravity()
    {
        // falling after jump
        if (isFinishedJumping) 
        {
            rb.gravityScale = gravityScale * fallGravityMultiplierAfterJump;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallingVelocity));
        }
        // hanging in the air after jump
        else if ((isJumping || isFallingAfterJump) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
        {
            rb.gravityScale = gravityScale * 0.5f;
        }
        // falling normally
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallingVelocity));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

    }

    void Update()
    {

        UpdateTimers();

        HandleInput();


        if (!isJumping)
        {
            UpdateCollisions();
        }

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            isFallingAfterJump = true;
        }

        if (isWallJumping)
        {
            isWallJumping = false;
        }

        if (lastGrounded > 0 && !isJumping)
        {
            isFinishedJumping = false;
            isFallingAfterJump = false;
        }

        if (CanWallJump() && lastPressedJump > 0)
        {
            isWallJumping = true;
            isJumping = false;
            isFinishedJumping = false;
            isFallingAfterJump = false;
            WallJump();

        } 
        else if (CanJump() && lastPressedJump > 0)
        {
            if (lastGrounded <= 0)
            {
                bonusJumps--;
            }

            isJumping = true;
            isWallJumping = false;
            isFinishedJumping = false;
            isFallingAfterJump = false;
            Jump();
        }

        UpdateGravity();
    }

    bool CanJump()
    {
        return bonusJumps > 0 || (lastGrounded > 0 && !isJumping);
    }

    bool CanWallJump()
    {
        return lastPressedJump > 0 && lastOnWall > 0 && lastGrounded <= 0;
    }

    void FixedUpdate()
    {
        Run(1);
    }
    void Run(float lerpAmount)
    {
        float velocity = input.x * maxVelocity;

        // Linear interpolation
        velocity = Mathf.Lerp(rb.velocity.x, velocity, lerpAmount);

        float acceletarion;

        // set acceleration based on if we're grounded or not
        if (lastGrounded > 0)
        {
            acceletarion = (Mathf.Abs(velocity) > 0.01f) ? trueAcceleration : trueDeceleration;
        }
        // apply a multiplier if we're in the air
        else
        {
            acceletarion = (Mathf.Abs(velocity) > 0.01f) ? trueAcceleration * airFriction : trueDeceleration * airFriction;
        }

        // apply a multiplier if we're falling 
        if ((isJumping || isFallingAfterJump) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
        {
            acceletarion *= jumpHangAccelerationMultiplier;
            velocity *= jumpHangMaxVelocityMultiplier;
        }

        // momentum conservation if we are moving faster than the max velocity
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(velocity) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(velocity) && Mathf.Abs(velocity) > 0.01f && lastGrounded < 0)
        {
            acceletarion = 0;
        }

        float dVelocity = velocity - rb.velocity.x;

        float distance = dVelocity * acceletarion;

        rb.AddForce(Vector2.right * distance, ForceMode2D.Force);
    }

    void Jump()
    {
        // Debug.Log("jumping");
        lastPressedJump = 0;
        lastGrounded = 0;

        float force = jumpingForce;
        if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void WallJump()
    {

        wallJumpDirection =  (lastOnWallRight > 0) ? -1 : 1;
        lastPressedJump = 0;
        lastGrounded = 0;
        lastOnWallLeft = 0;
        lastOnWallRight = 0;

        Vector2 force = new Vector2(wallJumpForce.x, wallJumpForce.y);
        force.x *= wallJumpDirection;

        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
        {
            force.x -= rb.velocity.x;
        }
        if (rb.velocity.y < 0)
        {
            force.y -= rb.velocity.y;
        }

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    void SetMovementParams()
    {
        gravityStrength = -(2 * jumpHeight) / Mathf.Pow(jumpApexTime, 2);
        gravityScale = gravityStrength / Physics2D.gravity.y;

        trueAcceleration = (50 * runningAcceleration) / maxVelocity;
        trueDeceleration = (50 * runningDeceleration) / maxVelocity;

        jumpingForce = Mathf.Abs(gravityStrength) * jumpApexTime;

        runningAcceleration = Mathf.Clamp(runningAcceleration, 0.01f, maxVelocity);
        runningDeceleration = Mathf.Clamp(runningDeceleration, 0.01f, maxVelocity);

    }
    void onValidate()
    {
        SetMovementParams();
    }

}
