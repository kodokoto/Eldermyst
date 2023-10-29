using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

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
	private float currentWalljumpSpeed;
	private float walljumpSpeedDecel;

    // controller state

    public bool grounded;
	public bool wasGrounded;
	public bool falling;
	public bool running;
    public bool jumping;
	public bool doubleJumping;
	public bool wallJumping;
	public bool wallJumpedRight;
	public bool touchingWall;
	public bool touchingWallR;
	public bool touchingWallL;
	public bool wallSliding;
	public bool wallSlidingR;
	public bool wallSlidingL;

    // constants

    public readonly float RUN_SPEED = 10f;
    public readonly float DEFAULT_GRAVITY_SCALE = 5f;
    public readonly float MAX_FALL_VELOCITY = 20f;
    public readonly float JUMP_SPEED = 16.65f;
    public readonly float MAX_JUMP_TIME = 9f;
	public float WALL_JUMP_SPEED = 16f;
	public float WALL_JUMP_TIME = 10f;
	public readonly float JUMP_BUFFER_TIME = 0.1f;
	private readonly float DOUBLE_JUMP_BUFFER_TIME = 10f;
	private readonly float FLOATING_BUFFER_TIME = 0.18f;
	private readonly int LEDGE_BUFFER_TIME = 2;
	private readonly int WALL_STICK_TIME = 3;



	// timers
	private float floatingBufferTimer;
	private int wallUnstickTimer;


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
		HandleBufferedInput();
		if (wallSliding)
		{
			HandleWallSliding();
		}
    }

    private void FixedUpdate()
    {
        Move(input);

		// if we are not touching the wall, flip the sprite
		if (!wallSliding && !wallJumping)
		{
			if (input != transform.right.x)
			{
				Flip();
			}
		}

        if (jumping)
        {
            Jump();
        }
		if (doubleJumping)
		{
			DoubleJump();
		}
		if (wallJumping)
		{
			WallJump();
		}
		if (wallSliding)
		{
			WallSlide();
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
    
		if (CanWallSlide())
		{
			if (touchingWallL && Input.GetKey(KeyCode.A) && !wallSliding)
			{
				doubleJumpAvailable = true;
				wallSliding = true;
				wallSlidingL = true;
				wallSlidingR = false;
				FaceLeft();
			}
			if (touchingWallR && Input.GetKey(KeyCode.D) && !wallSliding)
			{
				doubleJumpAvailable = false;
				wallSliding = true;
				wallSlidingL = false;
				wallSlidingR = true;
				FaceRight();
			}
		}

		if (wallSliding && Input.GetKeyDown(KeyCode.S))
		{
			CancelWallsliding();
			Flip();
		}
        if (Input.GetButtonDown("Jump"))
        {
            if (CanWallJump())
			{
				StartWallJump();
			}
			else if (CanJump())
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
    }

	private void HandleBufferedInput()
	{
		if (Input.GetButton("Jump"))
		{
			// Handle buffered inputs
			if (jumpBuffered && CanJump())
			{
				StartJump();
			}
			else if (doubleJumpBuffered && CanDoubleJump())
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
		// if velocity indicates that we are currently falling
        if (rb.velocity.y <= -1E-06f)
        {
			// if we are not touching the ground, we should be falling
            if (!TouchingGround())
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

		// if we have no vertical velocity, and we arent grounded, falling or jumping
		// then we are floating, if we are touching the ground we should be grounded
		// so we wait a bit before setting grounded to true
		if (rb.velocity.y == 0f && !grounded && !falling && !jumping)
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
				floatingBufferTimer = 0f;
			}
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
		doubleJumpBuffered = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime * MAX_JUMP_TIME);
		jumping = false;
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, JUMP_SPEED);
		ledgeBuffered = false;
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

	// ======== WALL JUMP ========

	private void StartWallJump()
	{
		if (touchingWallL)
		{
			FaceRight();
			wallJumpedRight = true;
		}
		else if (touchingWallR)
		{
			FaceLeft();
			wallJumpedRight = false;
		}
		CancelWallsliding();
		touchingWall = false;
		touchingWallL = false;
		touchingWallR = false;
		doubleJumpAvailable = true;
		currentWalljumpSpeed = WALL_JUMP_SPEED;
		walljumpSpeedDecel = (WALL_JUMP_SPEED - RUN_SPEED) / WALL_JUMP_TIME;
		StartJump();
		
		wallJumping = true;
		StartCoroutine(WallJumpTimer());
		jumpBuffered = false;
	}

	private IEnumerator WallJumpTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * WALL_JUMP_TIME);
		CancelWallJump();
	}

	private void WallJump()
	{
		rb.velocity = wallJumpedRight ? new Vector2(currentWalljumpSpeed, JUMP_SPEED) 
									  : new Vector2(-currentWalljumpSpeed, JUMP_SPEED);

		currentWalljumpSpeed -= walljumpSpeedDecel;
	}

	private void CancelWallJump()
	{
		if (wallJumping)
		{
			StopCoroutine(WallJumpTimer());
			wallJumping = false;
		}
	}

	private void HandleWallSliding()
	{
		doubleJumpAvailable = true;
		if (grounded)
		{
			Flip();
			CancelWallsliding();
		}
		if (!touchingWall)
		{
			Flip();
			CancelWallsliding();
		}
		if (!CanWallSlide())
		{
			CancelWallsliding();
		}
	}
	private void WallSlide()
	{
		if (wallSlidingL && Input.GetKeyDown(KeyCode.LeftArrow))
		{
			wallUnstickTimer++;
		}
		else if (wallSlidingR && Input.GetKeyDown(KeyCode.RightArrow))
		{
			wallUnstickTimer++;
		}
		else
		{
			wallUnstickTimer = 0;
		}
		if (wallUnstickTimer >= WALL_STICK_TIME)
		{
			CancelWallsliding();
		}
		{
			CancelWallsliding();
		}
		if (wallSlidingL)
		{
			if (!TouchingWall(CollisionSide.left, false))
			{
				Flip();
				CancelWallsliding();
			}
		}
		else if (wallSlidingR && !TouchingWall(CollisionSide.right, false))
		{
			Flip();
			CancelWallsliding();
		}
	}

	private void CancelWallsliding()
	{
		wallSliding = false;
		wallSlidingL = false;
		wallSlidingR = false;
		touchingWallL = false;
		touchingWallR = false;
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
		if (IsCollidingWithWall(collision) || IsCollidingWithGround(collision))
		{
			if (collisionSide == CollisionSide.top)
			{
				// headBumpTimer = HEAD_BUMP_STEPS * Time.fixedDeltaTime;
				if (jumping)
				{
					CancelJump();
					CancelDoubleJump();
				}
			}
			if (collisionSide == CollisionSide.bottom)
			{
				HandleGrounded();
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if  (IsCollidingWithWall(collision))
		{
			if (TouchingWall(CollisionSide.left, false))
			{
				touchingWall = true;
				touchingWallL = true;
				touchingWallR = false;
			}
			else if (TouchingWall(CollisionSide.right, false))
			{
				touchingWall = true;
				touchingWallL = false;
				touchingWallR = true;
			}
			else
			{
				StartCoroutine(LeaveWall());
			}
			if (TouchingGround())
			{
				if (falling)
				{
					HandleGrounded();
					return;
				}
			}
			else if (jumping || falling)
			{
				grounded = false;
				running = false;
				return;
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (touchingWallL && !TouchingWall(CollisionSide.left, false))
		{
			StartCoroutine(LeaveWallL());
		}
		if (touchingWallR && !TouchingWall(CollisionSide.right, false))
		{
			StartCoroutine(LeaveWallR());
		}
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

	private bool TouchingWall(CollisionSide side, bool checkTop = false)
	{

		LayerMask wallLayerMask = LayerMask.GetMask("StickyWall");
		float distance = 0.1f;
		float z = col.bounds.center.z;

		Vector3 topVec = side switch {
			CollisionSide.left => new(col.bounds.min.x, col.bounds.max.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.max.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 midVec = side switch {
			CollisionSide.left => new(col.bounds.min.x, col.bounds.center.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.center.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 bottomVec = side switch {
			CollisionSide.left => new(col.bounds.min.x, col.bounds.min.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.min.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 direction = side switch {
			CollisionSide.left => Vector2.left,
			CollisionSide.right => Vector2.right,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};
			
		float padding = 0.02f * Math.Sign(direction.x);

		topVec.x -= padding;
		midVec.x -= padding;
		bottomVec.x -= padding;

		Debug.DrawLine(topVec, topVec + direction * distance, Color.magenta, 0.15f);
		Debug.DrawLine(midVec, midVec + direction * distance, Color.magenta, 0.15f);
		Debug.DrawLine(bottomVec, bottomVec + direction * distance, Color.magenta, 0.15f);

        if (Physics.Raycast(midVec, direction, out RaycastHit midHit, distance, wallLayerMask))
		{
			if (!midHit.collider.isTrigger)
			{
				return true;
			}
		}
		if (Physics.Raycast(bottomVec, direction, out RaycastHit bottomHit, distance, wallLayerMask))
		{
			if (!bottomHit.collider.isTrigger)
			{
				return true;
			}
		}
		if (checkTop && Physics.Raycast(topVec, direction, out RaycastHit topHit, distance, wallLayerMask))
		{
			if (!topHit.collider.isTrigger)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsCollidingWithGround(Collision collision)
	{
		return collision.gameObject.layer == LayerMask.NameToLayer("Ground");
	}

	private bool IsCollidingWithWall(Collision collision)
	{
		return collision.gameObject.layer == LayerMask.NameToLayer("StickyWall");
	}

	IEnumerator LeaveWallL()
	{
		yield return new WaitForSeconds(0.1f);
		touchingWall = false;
		touchingWallL = false;
	}

	IEnumerator LeaveWallR()
	{
		yield return new WaitForSeconds(0.1f);
		touchingWall = false;
		touchingWallR = false;
	}

	IEnumerator LeaveWall()
	{
		yield return new WaitForSeconds(0.1f);
		touchingWall = false;
		touchingWallL = false;
		touchingWallR = false;
	}


	// ======== HELPER METHODS ========

	public void FaceRight()
	{
		transform.right = Vector3.right;
	}

	public void FaceLeft()
	{
		transform.right = Vector3.left;
	}

	public void Flip()
	{
		transform.right *= -1f;
	}

	private bool IsFacingRight()
	{
		return transform.right.x > 0f;
	}

	private bool CanJump()
	{
		if (wallSliding || jumping)
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
		return doubleJumpAvailable && !grounded && !wallSliding;
	}

	private bool CanWallJump()
	{
		return wallSliding || (touchingWall && !grounded);
	}

	private bool CanWallSlide()
	{
		return !grounded && (falling || wallSliding) && !doubleJumping;
	}

}