using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isJumpingHash;
    int isMagicHash;
    int isGrappleHash;
    int isFallingHash;
    PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isMagicHash = Animator.StringToHash("isMagic");
        isGrappleHash = Animator.StringToHash("isGrappling");
        isFallingHash = Animator.StringToHash("isFalling");
    }

    // Update is called once per frame
    void Update()
    {
        // if running and jumping, jump
        if ((Input.GetKey("a") || Input.GetKey("d")) && (Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isFallingHash, false);

        }

        // running
        if ((Input.GetKey("a") || Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);
            animator.SetBool(isFallingHash, false);

        }

        // idle
        if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);
            animator.SetBool(isFallingHash, false);
        }

        // jumping
        if ((!Input.GetKey("a")) && (!Input.GetKey("d"))&& (Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);
            animator.SetBool(isFallingHash, false);

        }

        // spellcasting
        if(Input.GetKey("e")|| Input.GetKey("c")| Input.GetKey("r")|| Input.GetKey("x")|| Input.GetKey("q"))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, true);
            animator.SetBool(isGrappleHash, false);
            animator.SetBool(isFallingHash, false);
        }

        // Wall jumping
        if ((movement.wallSliding || movement.wallJumping)||(Input.GetKey("space") && (movement.touchingWallL||movement.touchingWallR)))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, true);
            animator.SetBool(isFallingHash, false);
        }

        // falling
        if(movement.falling && !Input.GetKey("space") &&!(movement.wallJumping||movement.wallJumping))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);
            animator.SetBool(isFallingHash, true);
        }
    }
}
