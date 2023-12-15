using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isJumpingHash;
    int isMagicHash;
    int isGrappleHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isMagicHash = Animator.StringToHash("isMagic");
        isGrappleHash = Animator.StringToHash("isGrapple");
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

        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, false);
        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d"))&& (Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isMagicHash, false);

        }

        if(Input.GetKey("e")|| Input.GetKey("c")| Input.GetKey("r")|| Input.GetKey("x")|| Input.GetKey("q"))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isMagicHash, true);
        }

        //testing grapple
        if (Input.GetKey("g"))
        {
            animator.SetBool(isGrappleHash, true);
        }
    }
}
