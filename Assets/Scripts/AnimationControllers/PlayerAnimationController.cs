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
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey("a") || Input.GetKey("d")) && (Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isMagicHash, false);
        }

        if ((Input.GetKey("a") || Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);

        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);
        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d"))&& (Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, false);

        }

        if(Input.GetKey("e")|| Input.GetKey("c")| Input.GetKey("r")|| Input.GetKey("x")|| Input.GetKey("q"))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, true);
            animator.SetBool(isGrappleHash, false);
        }

        //testing grapple
        if (movement.wallSliding || movement.wallJumping || Input.GetKey("g"))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isGrappleHash, true);
            Debug.Log("Wall jump animation activated");
        }
    }
}
