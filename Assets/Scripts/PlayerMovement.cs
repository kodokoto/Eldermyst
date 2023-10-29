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
    public bool jumping;
    public bool falling;
    public bool running;

    // constants

    public readonly float RUN_SPEED = 10f;
    public readonly float DEFAULT_GRAVITY_SCALE = 5f;
    public readonly float MAX_FALL_VELOCITY = 20f;
    public readonly float JUMP_SPEED = 16.65f;
    public readonly float MAX_JUMP_TIME = 9f;

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

		// Apply gravity
		rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (gravityScale * Time.fixedDeltaTime * Physics.gravity.y));

		// Clamp velocity so that we dont fall faster than MAX_FALL_VELOCITY
		if (rb.velocity.y < -MAX_FALL_VELOCITY)
		{
			rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_VELOCITY);
		}
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
        }
        else if (Input.GetButtonUp("Jump"))
        {
            JumpReleased();
        }
    }

    private void HandleFalling()
    {
        if (rb.velocity.y < 0f)
        {
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
    }

	private void HandleGrounded()
	{
		falling = false;
		CancelJump();
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
		return false;
	}

}