using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum AnimationState {
    Running,
    Jumping,
    SpellCasting,
    Grappling,
    Falling,
    Idle
}

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    PlayerMovement movement;

    // need to do this because there is a bug in C# regarding enum properties recursivelly calling set
    // causing the stack to overflow and the program to crash
    private AnimationState _animationState { get; set;}

    AnimationState animationState {
        get { return _animationState; }
        set {
            foreach (AnimatorControllerParameter p in animator.parameters)
            {
                if (p.name == "" + value)
                {
                    animator.SetBool(p.name, true);
                }
                else
                {
                    animator.SetBool(p.name, false);
                }
            }
            _animationState = value;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if running and jumping, jump
        if ((Input.GetKey("a") || Input.GetKey("d")) && (Input.GetKey("space")))
        {
            animationState = AnimationState.Jumping;
        }

        // running
        if ((Input.GetKey("a") || Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animationState = AnimationState.Running;
        }

        // idle
        if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animationState = AnimationState.Idle;
        }

        // jumping
        if ((!Input.GetKey("a")) && (!Input.GetKey("d"))&& (Input.GetKey("space")) && !(Input.GetKey("e") || Input.GetKey("c") | Input.GetKey("r") || Input.GetKey("x") || Input.GetKey("q")))
        {
            animationState = AnimationState.Jumping;
        }

        // spellcasting
        if(Input.GetKey("e")|| Input.GetKey("c")| Input.GetKey("r")|| Input.GetKey("x")|| Input.GetKey("q"))
        {
            animationState = AnimationState.SpellCasting;
        }

        // Wall jumping
        if ((movement.wallSliding || movement.wallJumping)||(Input.GetKey("space") && (movement.touchingWallL||movement.touchingWallR)))
        {
            animationState = AnimationState.Grappling;
        }

        // falling
        if(movement.falling && !Input.GetKey("space") &&!(movement.wallJumping||movement.wallJumping))
        {
            animationState = AnimationState.Falling;
        }
    }
}
