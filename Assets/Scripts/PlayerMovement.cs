using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerMovementState
    {
        Jumping,
        MidAir,
        Falling,
        Grounded,
        OnWall,
        WallJumping
    }

    public Rigidbody rb;

    private PlayerMovementState state;
    public float fallSpeedYDampingChangeThreshold;

    public float jumpBufferTime = 0.1f;
    public float runSpeed;
    public float maxFallSpeed;


    // public Vector3 velocity;
    public Vector3 dVelocity;
    public float gravity;


    // Jumping
    public float jumpSpeed;
    public float maxJumpSpeed;
    public float midAirSpeed;
    public float jumpMaxDuration = 15/60f;
    public float jumpTimer;
    public bool hasDoubleJump = true;


    // wall jumping
    public float wallJumpDirection;
    public float wallFallSpeed; // should be 1/3 of fall speed
    public Vector2 wallJumpForce = new(15, 10);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = PlayerMovementState.Falling;
    }

    void Update()
    {
        // if user presses space, start jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space is down");
            StartJump();
        }
        // if user releases space, stop jumping
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Stop jump from release");
            StopJump(true);
        }

        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }

    }
    void FixedUpdate()
    {
        Debug.Log("Start frame ");
        Move();
        switch (state)
        {
            case PlayerMovementState.Jumping:
                Jump();
                break;
            case PlayerMovementState.WallJumping:
                WallJump();
                break;
            case PlayerMovementState.MidAir:
                MidAir();
                break;
            default:
                break;
        }
        Gravity();
        ResolveMovement();
        Debug.Log("End frame");
    }

    void MidAir()
    {
        AddForce(Vector3.up, midAirSpeed);
    }

    IEnumerator JumpTimer()
    {
        // wait for 15 frames then call StopJump
        yield return new WaitForSeconds(jumpMaxDuration);
        if (state.Equals(PlayerMovementState.Jumping))
        {
            Debug.Log("Stop jump from coroutine");
            StopJump();
        }
    }

    IEnumerator MidAirTimer()
    {
        // wait for 15 frames then call StopJump
        yield return new WaitForSeconds(jumpMaxDuration);
        if (state.Equals(PlayerMovementState.MidAir))
        {
            Debug.Log("Changed state from MidAirTimer to Falling");
            ChangeMovementState(PlayerMovementState.Falling);
        }
    }

    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(jumpMaxDuration * 2);
        if (state.Equals(PlayerMovementState.WallJumping))
        {
            Debug.Log("Changed state from WallJumpTimer to Falling");
            ChangeMovementState(PlayerMovementState.Falling);
        }
    }

    void StopAllMovementTimers()
    {
        StopCoroutine(JumpTimer());
        StopCoroutine(MidAirTimer());
        StopCoroutine(WallJumpTimer());
    }

    void Move()
    {
        if (!state.Equals(PlayerMovementState.OnWall))
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            AddForce(Vector3.right, xInput * runSpeed);
        }
    }

    void Gravity()
    {
        Vector3 direction = state switch
        {
            PlayerMovementState.OnWall => Vector3.left * wallJumpDirection,
            _ => Vector3.down,
        };

        var scale = state switch
        {
            PlayerMovementState.Falling => gravity,
            PlayerMovementState.OnWall => wallFallSpeed,
            // PlayerMovementState.MidAir => midAirSpeed,
            _ => 0,
        };
        Debug.Log("Gravity " + direction * scale);
        AddForce(direction, scale);
    }

    bool IsInAir()
    {
        return state.Equals(PlayerMovementState.Falling) || state.Equals(PlayerMovementState.Jumping) || state.Equals(PlayerMovementState.MidAir);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Changed state from OnCollisionEnter to Grounded");
            ChangeMovementState(PlayerMovementState.Grounded);
            hasDoubleJump = true;
        }
        else if (collision.gameObject.CompareTag("StickyWall") && !state.Equals(PlayerMovementState.Grounded))
        {
            Debug.Log("Changed state from OnCollisionEnter to OnWall");
            bool res = ChangeMovementState(PlayerMovementState.OnWall);
            if (res)
            {
                // use collision normal to determine which direction to jump
                wallJumpDirection = collision.contacts[0].normal.x;
                hasDoubleJump = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !state.Equals(PlayerMovementState.Jumping) && !Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            Debug.Log("Changed state from OnCollisionExit to Falling");
            ChangeMovementState(PlayerMovementState.Falling);
        }
        else if (collision.gameObject.CompareTag("StickyWall") && state.Equals(PlayerMovementState.OnWall))
        {
            ChangeMovementState(PlayerMovementState.Falling);
        }
    }


    // void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         ChangeMovementState(PlayerMovementState.Grounded);
    //     }
    //     else if (collision.gameObject.CompareTag("StickyWall"))
    //     {
    //         ChangeMovementState(PlayerMovementState.OnWall);
    //     }
    // }

    // JUMPING

    IEnumerator JumpInputBuffer()
    {
        float bufferTime = 0.1f; // Adjust the time window as needed

        // Wait for the specified buffer time
        yield return new WaitForSeconds(bufferTime);

        if (state == PlayerMovementState.Grounded)
        {
            StartJump();
        }
    }

    IEnumerator CoyoteBuffer()
    {
        float bufferTime = 0.1f; // Adjust the time window as needed

        // Wait for the specified buffer time
        yield return new WaitForSeconds(bufferTime);

        if (state != PlayerMovementState.Jumping)
        {
            ChangeMovementState(PlayerMovementState.Falling);
        }
    }

    void StartJump()
    {
        if (state.Equals(PlayerMovementState.Grounded))
        {
            StopCoroutine(JumpTimer());
            ChangeMovementState(PlayerMovementState.Jumping);
        } 
        else if (IsInAir() && hasDoubleJump)
        {
            if (state.Equals(PlayerMovementState.Jumping))
            {  
                StopCoroutine(JumpTimer());
            } else if (state.Equals(PlayerMovementState.WallJumping))
            {
                StopCoroutine(WallJumpTimer());
            } 
            ChangeMovementState(PlayerMovementState.Jumping);
            hasDoubleJump = false;
        }
        else if (state.Equals(PlayerMovementState.Falling))
        {
            StartCoroutine(JumpInputBuffer());
        }
        else if (state.Equals(PlayerMovementState.OnWall))
        {
            WallJump();
            ChangeMovementState(PlayerMovementState.WallJumping);
        }
    }

    void Jump()
    {
        AddForce(Vector3.up, jumpSpeed);
    }


    void WallJump()
    {
        Debug.Log("Wall Jump");
        if (Input.GetAxisRaw("Horizontal") != wallJumpDirection)
        {
            AddForce(Vector3.right, wallJumpDirection * wallJumpForce.x / 2);
        } else
        {
            AddForce(Vector3.right, wallJumpDirection * wallJumpForce.x);
        }
        AddForce(Vector3.up, wallJumpForce.y);
    }

    void StopJump(bool sharp = false)
    {
        if (state.Equals(PlayerMovementState.Jumping))
        {
            
            if (sharp)
            {
                Debug.Log("Sharp Jump");
                AddForce(Vector3.down, Mathf.Abs((rb.velocity.y / Time.fixedDeltaTime) / 1.5f));
                ChangeMovementState(PlayerMovementState.Falling);
            } else {
                ChangeMovementState(PlayerMovementState.MidAir);
            }
        }
    }


    bool ChangeMovementState(PlayerMovementState newState)
    {
        bool completed = true;
        switch (newState)
        {
            case PlayerMovementState.Jumping:
                StartCoroutine(JumpTimer());
                state = newState;
                break;
            case PlayerMovementState.MidAir:
                StartCoroutine(MidAirTimer());
                state = newState;
                break;
            case PlayerMovementState.Falling:
                StopAllMovementTimers();
                state = newState;
                break;
            case PlayerMovementState.Grounded:
                StopAllMovementTimers();
                state = newState;
                break;
            case PlayerMovementState.OnWall:
                StopAllMovementTimers();
                state = newState;
                break;
            case PlayerMovementState.WallJumping:
                StartCoroutine(WallJumpTimer());
                state = newState;
                break;

        }
        return completed;
    }

    void AddForce(Vector3 direction, float scale)
    {
        dVelocity += direction * (scale); 
    }



    void ResolveMovement()
    {
        Vector3 targetVelocity = dVelocity;

        float minVerticalVelocity = state switch {
            PlayerMovementState.Falling => -1*maxFallSpeed,
            PlayerMovementState.OnWall => wallFallSpeed,
            _ => 0,
        };

        // clamp vertical velocity
        targetVelocity.y = Mathf.Clamp(targetVelocity.y, minVerticalVelocity, maxJumpSpeed);

        // // move the player
        // targetVelocity.x -= rb.velocity.x;
        // rb.AddForce(targetVelocity, ForceMode.Impulse);

        // move the player

        // // if the player is changing direction (signs are different), ignore the current horizontal velocity
        // if (Math.Sign(targetVelocity.x) != Math.Sign(rb.velocity.x))
        // {
        //     targetVelocity.x = dVelocity.x;
        // }

        // targetVelocity *= Time.fixedDeltaTime * 100;
        rb.velocity = targetVelocity * 2;
        Debug.Log("Target Velocity " + targetVelocity);
        Debug.Log("State " + state);

        // reset the dVelocity
        dVelocity = Vector3.zero;

        // update the movement state if jumping or rising
        // if (state.Equals(PlayerMovementState.Jumping) || state.Equals(PlayerMovementState.MidAir))
        // {
        //     if (targetVelocity.y <= 0)
        //     {
        //         ChangeMovementState(PlayerMovementState.Falling);
        //     }
        // }

        // else if (state.Equals(PlayerMovementState.Falling))
        // {
        //     if (velocity.y >= 0)
        //     {
        //         ChangeMovementState(PlayerMovementState.MidAir);
        //     }
        // }
    }
}