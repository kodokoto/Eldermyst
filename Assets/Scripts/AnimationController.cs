using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isJumpingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey("a") || Input.GetKey("d")) && (Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
        }

        if ((Input.GetKey("a") || Input.GetKey("d")) && (!Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, true);
            animator.SetBool(isJumpingHash, false);
        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, false);
        }

        if ((!Input.GetKey("a")) && (!Input.GetKey("d"))&& (Input.GetKey("space")))
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isJumpingHash, true);
        }
    }
}
