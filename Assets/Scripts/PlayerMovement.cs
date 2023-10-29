using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CollisionSide
{
	left,
	right,
	top,
	bottom
}

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider col;

    // ingame variables
    public float input;
    public float gravityScale;
	public bool doubleJumpAvailable = true;

    // controller state

    public bool grounded;
	public bool wasGrounded;
	public bool falling;
	public bool running;
    public bool jumping;
	public bool doubleJumping;


    // constants

    public readonly float RUN_SPEED = 10f;
    public readonly float DEFAULT_GRAVITY_SCALE = 5f;
    public readonly float MAX_FALL_VELOCITY = 20f;
    public readonly float JUMP_SPEED = 16.65f;
    public readonly float MAX_JUMP_TIME = 9f;

	public readonly float JUMP_BUFFER_TIME = 0.1f;
	private readonly int DOUBLE_JUMP_BUFFER_TIME = 10;
	private readonly float FLOATING_BUFFER_TIME = 0.18f;
	private readonly int LEDGE_BUFFER_TIME = 2;


	// timers
	private float floatingBufferTimer;
	private float ledgeBufferTimer;

	// buffer states
	private bool jumpBuffered;
	private bool doubleJumpBuffered;
	private bool ledgeBuffered;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        gravityScale = DEFAULT_GRAVITY_SCALE;
    }

    private void Update()
    {
        HandleFalling();
        HandleInput();

    }

    private void FixedUpdate()
    {
        Move(input);

        if (jumping)
        {
            Jump();
        }
		if (doubleJumping)
		{
			DoubleJump();
		}

		// Apply gravity
		rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (gravityScale * Time.fixedDeltaTime * Physics.gravity.y));

		// Clamp velocity so that we dont fall faster than MAX_FALL_VELOCITY
		if (rb.velocity.y < -MAX_FALL_VELOCITY)
		{
			rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_VELOCITY);
		}

		wasGrounded = grounded;
    }

    private void HandleInput()
    {
        input = Input.GetAxisRaw("Horizontal");
    
        if (Input.GetButtonDown("Jump"))
        {
            if (CanJump())
			{
				StartJump();
			}
			else if (CanDoubleJump())
			{
				StartDoubleJump();
			}
			else
			{
				HanldeJumpBuffer();
				HandleDoubleJumpBuffer();
			}
        }
        else if (Input.GetButtonUp("Jump"))
        {
            JumpReleased();
        }
		else if (Input.GetButton("Jump"))
		{
			// Handle buffered inputs
			if (jumpBuffered)
			{
				StartJump();
			}
			else if (doubleJumpBuffered)
			{
				if (grounded)
				{
					StartJump();
				}
				else
				{
					StartDoubleJump();
				}
			}
		}
    }

    private void HandleFalling()
    {
        if (rb.velocity.y < 0f)
        {
            if (TouchingGround())
			{
				floatingBufferTimer += Time.deltaTime;
				if (floatingBufferTimer > FLOATING_BUFFER_TIME)
				{
					HandleGrounded();
					floatingBufferTimer = 0f;
					return;
				}
			}
			else
            {
                falling = true;
                grounded = false;
                running = false;
            }
        }
        else
        {
            falling = false;
        }
    }

	private void HandleGrounded()
	{
		falling = false;
		CancelJump();
		if (doubleJumping)
		{
			StartDoubleJump();
		}
		if (Mathf.Abs(input) > Mathf.Epsilon)
		{
			running = true;
		}
		grounded = true;
		doubleJumpAvailable = true;
	}


	// ======== MOVEMENT FUNCTIONS ========

	private void Move(float move_direction)
	{
		if (grounded && Mathf.Abs(input) > Mathf.Epsilon)
		{
			running = true;
		}
		else
		{
			running = false;
		}
		rb.velocity = new Vector2(move_direction * RUN_SPEED, rb.velocity.y);
	}


	// ======== JUMP ========

	void StartJump()
	{
	    jumping = true;
		StartCoroutine(JumpTimer());
	}

	IEnumerator JumpTimer()
	{
        yield return new WaitForSeconds(Time.fixedDeltaTime * MAX_JUMP_TIME);
		jumping = false;
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, JUMP_SPEED);
	}

	private void CancelJump()
	{
		if (jumping)
		{
			StopCoroutine(JumpTimer());
			jumping = false;
		}
	}

    private void JumpReleased()
	{
		if (rb.velocity.y > 0f)
		{
			rb.velocity = new Vector2(rb.velocity.x, 0f);
			CancelJump();
		}
		jumpBuffered = false;
		doubleJumpBuffered = false;
	}

	

	// ======== DOUBLE JUMP ========

	private void StartDoubleJump()
	{
		CancelJump();
		doubleJumping = true;
		StartCoroutine(DoubleJumpTimer());
		doubleJumpAvailable = false;
	}

	IEnumerator DoubleJumpTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * MAX_JUMP_TIME);
		doubleJumping = false;
	}

	private void DoubleJump()
	{
		rb.velocity = new Vector2(rb.velocity.x, JUMP_SPEED * 1.1f);
		if (grounded)
		{
			CancelDoubleJump();
		}
	}

	private void CancelDoubleJump()
	{
		if (doubleJumping)
		{
			StopCoroutine(DoubleJumpTimer());
			doubleJumping = false;
		}
	}

	// ======== BUFFERED INPUTS ========

	private void HanldeJumpBuffer()
	{
		CancelJumpBuffer();
		jumpBuffered = true;
		StartCoroutine(JumpBufferTimer());
	}

	private void CancelJumpBuffer()
	{
		if (jumpBuffered)
		{
			StopCoroutine(JumpBufferTimer());
			jumpBuffered = false;
		}
	}

	private IEnumerator JumpBufferTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * JUMP_BUFFER_TIME);
		jumpBuffered = false;
	}

	private void HandleDoubleJumpBuffer()
	{
		CancelDoubleJumpBuffer();
		doubleJumpBuffered = true;
		StartCoroutine(DoubleJumpBufferTimer());
	}

	private void CancelDoubleJumpBuffer()
	{
		if (doubleJumpBuffered)
		{
			StopCoroutine(DoubleJumpBufferTimer());
			doubleJumpBuffered = false;
		}
	}

	private IEnumerator DoubleJumpBufferTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * DOUBLE_JUMP_BUFFER_TIME);
		doubleJumpBuffered = false;
	}

	private void HandleLedgeBuffer()
	{
		CancelLedgeBuffer();
		ledgeBuffered = true;
		StartCoroutine(LedgeBufferTimer());
	}

	private void CancelLedgeBuffer()
	{
		if (ledgeBuffered)
		{
			StopCoroutine(LedgeBufferTimer());
			ledgeBuffered = false;
		}
	}

	private IEnumerator LedgeBufferTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * LEDGE_BUFFER_TIME);
		ledgeBuffered = false;
	}

    // ======== COLLISIONS ========

	private void OnCollisionEnter(Collision collision)
	{
		CollisionSide collisionSide = FindCollisionDirection(collision);
		if (IsCollidingWithGround(collision))
		{
			if (collisionSide == CollisionSide.bottom)
			{
				HandleGrounded();
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if  (IsCollidingWithWall(collision) && !TouchingGround())
		{
			grounded = false;
			running = false;
			if (wasGrounded)
			{
				HandleLedgeBuffer();
			}
		}
	}

	private CollisionSide FindCollisionDirection(Collision collision)
	{
		Vector2 normal = collision.contacts[0].normal;
		float x = normal.x;
		float y = normal.y;
		if (y >= 0.5f)
		{
			return CollisionSide.bottom;
		}
		if (y <= -0.5f)
		{
			return CollisionSide.top;
		}
		if (x < 0f)
		{
			return CollisionSide.right;
		}
		if (x > 0f)
		{
			return CollisionSide.left;
		}
		Debug.LogError(string.Concat(new string[]
		{
			"ERROR: unable to determine direction of collision - contact points at (",
			normal.x.ToString(),
			",",
			normal.y.ToString(),
			")"
		}));
		return CollisionSide.bottom;
	}		
    
    public bool TouchingGround()
	{
		LayerMask groundLayer = LayerMask.GetMask("Ground");
		float z = col.bounds.center.z;
		Vector3 midLeft = new(col.bounds.min.x, col.bounds.center.y, z);
		Vector3 mid = col.bounds.center;
		Vector3 midRight = new(col.bounds.max.x, col.bounds.center.y, z);
		float distance = col.bounds.extents.y + 0.16f;

		Debug.DrawRay(midLeft, Vector2.down, Color.yellow);
		Debug.DrawRay(mid, Vector2.down, Color.yellow);
		Debug.DrawRay(midRight, Vector2.down, Color.yellow);

		return Physics.Raycast(mid, Vector2.down, distance, groundLayer) || Physics.Raycast(midLeft, Vector2.down, distance, groundLayer) || Physics.Raycast(midRight, Vector2.down, distance, groundLayer);
	}

	private bool IsCollidingWithGround(Collision collision)
	{
		return collision.gameObject.layer == LayerMask.NameToLayer("Ground");
	}

	private bool IsCollidingWithWall(Collision collision)
	{
		return collision.gameObject.layer == LayerMask.NameToLayer("StickyWall");
	}


	// ======== HELPER METHODS ========

	private bool CanJump()
	{
		if (jumping)
		{
			return false;
		}
		if (grounded)
		{
			return true;
		}
		if (ledgeBuffered)
		{
			ledgeBuffered = false;
			return true;
		}
		return false;
	}

	private bool CanDoubleJump()
	{
		return doubleJumpAvailable && !grounded;
	}

}