using System.Collections;
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
	private Rigidbody rb;
	private CapsuleCollider col;
	// ingame variables
	private float input;
	private float gravityScale;
	private bool doubleJumpAvailable = true;
	private float currentWalljumpSpeed;
	private float walljumpSpeedDecel;
	private bool dashAvailable = true;
	private float currentSpeed { get; set; }


	[Space(5)]
	[Header("Movement States")]

	public bool grounded;
	public bool wasGrounded;
	public bool falling;
	public bool running;
	public bool jumping;
	public bool doubleJumping;
	public bool wallJumping;
	public bool wallJumpedRight;
	public bool dashing;
	public bool touchingWall;
	public bool touchingWallR;
	public bool touchingWallL;
	public bool wallSliding;
	public bool wallSlidingR;
	public bool wallSlidingL;

	// constants

	private readonly float RUN_SPEED = 10f;
	private readonly float DEFAULT_GRAVITY_SCALE = 5f;
	private readonly float MAX_FALL_VELOCITY = 20f;
	private readonly float JUMP_SPEED = 16.65f;
	private readonly float MAX_JUMP_TIME = 9f;
	private readonly float WALL_JUMP_SPEED = 16f;
	private readonly float WALL_JUMP_TIME = 10f;
	private readonly float JUMP_BUFFER_TIME = 0.1f;
	private readonly float DOUBLE_JUMP_BUFFER_TIME = 10f;
	private readonly float FLOATING_BUFFER_TIME = 0.18f;
	private readonly int LEDGE_BUFFER_TIME = 2;
	private readonly int WALL_STICK_TIME = 3;
	private readonly float DASH_SPEED = 20f;
	private readonly float DASH_TIME = 4;
	private readonly int DASH_BUFFER_TIME = 10;
	private readonly float DASH_COOLDOWN = 0.6f;
	private readonly float BUMP_VELOCITY = 4f;
	private readonly float BUMP_VELOCITY_DASH = 5f;
	private readonly int HEAD_BUMP_STEPS = 3;



	// timers
	private float floatingBufferTimer;
	private int wallUnstickTimer;

	// buffer states
	private bool jumpBuffered;
	private bool doubleJumpBuffered;
	private bool dashBuffered;

	// Coroutine references
	private bool ledgeBuffered;
	private bool headBumpBuffered;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
		gravityScale = DEFAULT_GRAVITY_SCALE;
		currentSpeed = RUN_SPEED;
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
		if (!dashing)
		{
			Move(input);

			// if we are not touching the wall, flip the sprite
			if (!wallSliding && !wallJumping)
			{
				if (input > 0f && !IsFacingRight())
				{
					Flip();
				}
				else if (input < 0f && IsFacingRight())
				{
					Flip();
				}
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
		if (dashing)
		{
			Dash();
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
			if (touchingWallL && Input.GetKey(KeyCode.LeftArrow) && !wallSliding)
			{
				doubleJumpAvailable = true;
				wallSliding = true;
				wallSlidingL = true;
				wallSlidingR = false;
				FaceLeft();
			}
			if (touchingWallR && Input.GetKey(KeyCode.RightArrow) && !wallSliding)
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
				HanldeBufferedJump();
				HandleBufferedDoubleJump();
			}
		}
		else if (Input.GetButtonUp("Jump"))
		{
			JumpReleased();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			if (CanDash())
			{
				StartDash();
			}
			else
			{
				HandleBufferedDash();
			}
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
		if (Input.GetKeyDown(KeyCode.LeftShift) && dashBuffered && dashAvailable)
		{
			if (CanDash())
			{
				StartDash();
			}
			else
			{
				HandleBufferedDash();
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
		if (rb.velocity.y == 0f && !grounded && !falling && !jumping && !dashing)
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
		if (!wallSliding)
		{
			rb.velocity = new Vector2(move_direction * currentSpeed, rb.velocity.y);
		}
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
		walljumpSpeedDecel = (WALL_JUMP_SPEED - currentSpeed) / WALL_JUMP_TIME;
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

	// ======== DASH ========

	private void StartDash()
	{
		if (wallSliding)
		{
			Flip();
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			FaceRight();
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			FaceLeft();
		}
		dashing = true;
		dashBuffered = false;
		HanldeDashCooldown();
		StartCoroutine(DashTimer());
	}

	IEnumerator DashTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * DASH_TIME);
		FinishedDashing();
	}

	private void Dash()
	{
		AffectedByGravity(false);
		float num = DASH_SPEED;
		if (IsFacingRight())
		{
			if (CheckForBump(CollisionSide.right))
			{
				rb.velocity = new Vector2(num, grounded ? BUMP_VELOCITY : BUMP_VELOCITY_DASH);
			}
			else
			{
				rb.velocity = new Vector2(num, 0f);
			}
		}
		else if (CheckForBump(CollisionSide.left))
		{
			rb.velocity = new Vector2(-num, grounded ? BUMP_VELOCITY : BUMP_VELOCITY_DASH);
		}
		else
		{
			rb.velocity = new Vector2(-num, 0f);
		}
	}

	private void FinishedDashing()
	{
		CancelDash();
		AffectedByGravity(true);
		if (touchingWall && !grounded && (touchingWallL || touchingWallR))
		{
			wallSliding = true;
			if (touchingWallL)
			{
				wallSlidingL = true;
			}
			if (touchingWallR)
			{
				wallSlidingR = true;
			}
		}
	}

	private void CancelDash()
	{
		dashing = false;
		StopCoroutine(DashTimer());
		AffectedByGravity(true);
	}

	// ======== COLLISIONS ========

	private void OnCollisionEnter(Collision collision)
	{
		CollisionSide collisionSide = FindCollisionDirection(collision);
		bool collidedWithGround = IsCollidingWithGround(collision);
		if (IsCollidingWithWall(collision) || collidedWithGround)
		{
			if (collisionSide == CollisionSide.top)
			{
				HandleHeadBump();
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
		if (collidedWithGround)
		{
			// check if object is MovingPlatform
			if (collision.gameObject.GetComponent<MovingPlatform>() != null)
			{
				// if so, set player's parent to the MovingPlatform
				transform.parent = collision.gameObject.transform;
			}
		}

	}

	private void OnCollisionStay(Collision collision)
	{
		if (IsCollidingWithWall(collision))
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
		if (IsCollidingWithWall(collision) && !TouchingGround())
		{
			grounded = false;
			running = false;
			if (wasGrounded)
			{
				HandleLedgeBuffer();
			}
		}
		if (IsCollidingWithGround(collision))
		{
			// check if object is MovingPlatform
			if (collision.gameObject.GetComponent<MovingPlatform>() != null)
			{
				// if so, set player's parent to the MovingPlatform
				transform.parent = null;
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

	private bool TouchingGround()
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

		Vector3 topVec = side switch
		{
			CollisionSide.left => new(col.bounds.min.x, col.bounds.max.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.max.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 midVec = side switch
		{
			CollisionSide.left => new(col.bounds.min.x, col.bounds.center.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.center.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 bottomVec = side switch
		{
			CollisionSide.left => new(col.bounds.min.x, col.bounds.min.y, z),
			CollisionSide.right => new(col.bounds.max.x, col.bounds.min.y, z),
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Vector3 direction = side switch
		{
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

	private bool CheckForBump(CollisionSide side)
	{
		float num = 0.025f;
		float num2 = 0.2f;
		float num3 = 0.32f + num2;

		LayerMask wallLayerMask = LayerMask.GetMask("StickyWall");
		float z = col.bounds.center.z;

		Vector3 vector = new(col.bounds.min.x + num2, col.bounds.min.y + 0.2f, z);
		Vector3 vector2 = new(col.bounds.min.x + num2, col.bounds.min.y - num, z);
		Vector3 vector3 = new(col.bounds.max.x - num2, col.bounds.min.y + 0.2f, z);
		Vector3 vector4 = new(col.bounds.max.x - num2, col.bounds.min.y - num, z);

		Vector3 vec = side switch
		{
			CollisionSide.left => vector,
			CollisionSide.right => vector3,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};
		Vector3 vec2 = side switch
		{
			CollisionSide.left => vector2,
			CollisionSide.right => vector4,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};
		Vector3 direction = side switch
		{
			CollisionSide.left => Vector2.left,
			CollisionSide.right => Vector2.right,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid CollisionSide specified.")
		};

		Debug.DrawLine(vec2, vec2 + direction * num3, Color.cyan, 0.15f);
		Debug.DrawLine(vec, vec + direction * num3, Color.cyan, 0.15f);

		if (Physics.Raycast(vec2, direction, out RaycastHit hit2, num3, wallLayerMask) && !Physics.Raycast(vec, direction, out RaycastHit hit, num3, wallLayerMask))
		{
			Vector3 vector5 = hit2.point + new Vector3((side == CollisionSide.right) ? 0.1f : (-0.1f), 1f, z);
			Vector3 vector6 = hit2.point + new Vector3((side == CollisionSide.right) ? (-0.1f) : 0.1f, 1f, z);
			if (Physics.Raycast(vector5, Vector2.down, out RaycastHit hit3, 1.5f, wallLayerMask))
			{
				Debug.DrawLine(vector5, hit3.point, Color.cyan, 0.15f);
				if (!Physics.Raycast(vector6, Vector2.down, out RaycastHit hit4, 1.5f, wallLayerMask))
				{
					return true;
				}
				Debug.DrawLine(vector6, hit4.point, Color.cyan, 0.15f);
				float num4 = hit3.point.y - hit4.point.y;
				if (num4 > 0f)
				{
					Debug.Log("Bump Height: " + num4.ToString());
					return true;
				}
			}
		}
		return false;
	}

	private bool CheckNearRoof()
	{
		LayerMask wallLayerMask = LayerMask.GetMask("StickyWall");
		Vector2 vector = col.bounds.max;
		Vector2 vector2 = new(col.bounds.min.x, col.bounds.max.y);
		new Vector2(col.bounds.center.x, col.bounds.max.y);
		Vector2 vector3 = new(col.bounds.center.x + col.bounds.size.x / 4f, col.bounds.max.y);
		Vector2 vector4 = new(col.bounds.center.x - col.bounds.size.x / 4f, col.bounds.max.y);
		Vector2 vector5 = new(-0.5f, 1f);
		Vector2 vector6 = new(0.5f, 1f);
		Vector2 up = Vector2.up;
		Physics.Raycast(vector2, vector5, out RaycastHit hit, 2f, wallLayerMask);
		Physics.Raycast(vector, vector6, out RaycastHit hit2, 2f, wallLayerMask);
		Physics.Raycast(vector3, up, out RaycastHit hit3, 1f, wallLayerMask);
		Physics.Raycast(vector4, up, out RaycastHit hit4, 1f, wallLayerMask);
		return hit.collider != null || hit2.collider != null || hit3.collider != null || hit4.collider != null;
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

	private void FaceRight()
	{
		transform.right = transform.right;
	}

	private void FaceLeft()
	{
		transform.right = -transform.right;
	}

	private void Flip()
	{
		transform.right *= -1f;
	}

	private bool IsFacingRight()
	{
		return transform.right.x > 0f;
	}

	public void AffectedByGravity(bool gravityApplies)
	{
		if (gravityScale > Mathf.Epsilon && !gravityApplies)
		{
			gravityScale = 0f;
			return;
		}
		if (gravityScale <= Mathf.Epsilon && gravityApplies)
		{
			gravityScale = DEFAULT_GRAVITY_SCALE;
		}
	}

	private bool CanJump()
	{
		if (wallSliding || dashing || jumping)
		{
			return false;
		}
		if (grounded)
		{
			return true;
		}
		if (ledgeBuffered && headBumpBuffered && !CheckNearRoof())
		{
			ledgeBuffered = false;
			return true;
		}
		return false;
	}

	private bool CanDoubleJump()
	{
		return doubleJumpAvailable && !dashing && !wallSliding && !grounded;
	}

	private bool CanDash()
	{
		return dashAvailable && !dashing && (grounded || wallSliding);
	}

	private bool CanWallSlide()
	{
		return !dashing && !grounded && (falling || wallSliding) && !doubleJumping;
	}

	private bool CanWallJump()
	{
		return wallSliding || (touchingWall && !grounded);
	}

	// ======== BUFFERED INPUTS ========

	private void HanldeBufferedJump()
	{
		CancelBufferedJump();
		jumpBuffered = true;
		StartCoroutine(BufferedJumpTimer());
	}

	private void CancelBufferedJump()
	{
		if (jumpBuffered)
		{
			StopCoroutine(BufferedJumpTimer());
			jumpBuffered = false;
		}
	}

	private IEnumerator BufferedJumpTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * JUMP_BUFFER_TIME);
		jumpBuffered = false;
	}

	private void HandleBufferedDoubleJump()
	{
		CancelBufferedDoubleJump();
		doubleJumpBuffered = true;
		StartCoroutine(DoubleBufferedJumpTimer());
	}

	private void CancelBufferedDoubleJump()
	{
		if (doubleJumpBuffered)
		{
			StopCoroutine(DoubleBufferedJumpTimer());
			doubleJumpBuffered = false;
		}
	}

	private IEnumerator DoubleBufferedJumpTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * DOUBLE_JUMP_BUFFER_TIME);
		doubleJumpBuffered = false;
	}

	private void HandleBufferedDash()
	{
		CancelBufferedDash();
		dashBuffered = true;
		StartCoroutine(BufferedDashTimer());
	}

	private void CancelBufferedDash()
	{
		if (dashBuffered)
		{
			StopCoroutine(BufferedDashTimer());
			dashBuffered = false;
		}
	}

	private IEnumerator BufferedDashTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * DASH_BUFFER_TIME);
		dashBuffered = false;
	}


	// ======== COROUTINE TIMERS ========

	private void HanldeDashCooldown()
	{
		CancelDashCooldown();
		dashAvailable = false;
		StartCoroutine(DashCooldownTimer());
	}

	private void CancelDashCooldown()
	{
		if (!dashAvailable)
		{
			StopCoroutine(DashCooldownTimer());
			dashAvailable = true;
		}
	}

	private IEnumerator DashCooldownTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * DASH_COOLDOWN);
		dashAvailable = true;
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

	private void HandleHeadBump()
	{
		CancelHeadBump();
		headBumpBuffered = true;
		StartCoroutine(HeadBumpTimer());
	}

	private void CancelHeadBump()
	{
		if (headBumpBuffered)
		{
			StopCoroutine(HeadBumpTimer());
			headBumpBuffered = false;
		}
	}

	private IEnumerator HeadBumpTimer()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * HEAD_BUMP_STEPS);
		headBumpBuffered = false;
	}


	// ======== Public interface ========

	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}

	public void SetCurrentSpeed(float speed)
	{
		currentSpeed = speed;
	}
}