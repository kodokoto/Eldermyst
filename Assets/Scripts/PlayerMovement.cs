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


    // ground

    private float lastGrounded;

    // Jumping
    private bool isFinishedJumping;
    private bool isFallingAfterJump;



    // Collision checks
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Transform frontChck;
	[SerializeField] private Transform backCheck;


    // movement parameters
    private float gravityScale;
    private float gravityStrength;
    private float jumpingForce;

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
    void Update()
    {
        lastGrounded -= Time.deltaTime;

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

        if (!isJumping)
        {
            // Grounded
            if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
            {
                lastGrounded = coyoteTime;
            }
        }

        if (lastGrounded > 0 && !isJumping)
        {
            isFinishedJumping = false;
            isFallingAfterJump = false;
        }

        // GRAVITY

        // falling after jump
        if (isFinishedJumping) 
        {
            rb.gravityScale = gravityScale * fallGravityMultiplierAfterJump;
            rb.velocity += new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallingVelocity));
        }
        // falling normally
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.velocity += new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallingVelocity));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
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
        if (isFallingAfterJump && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
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
