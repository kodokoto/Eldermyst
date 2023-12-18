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

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerMovement movement;

    [SerializeField] private AnimationState _animationState;

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

        // idle
        if (movement.grounded)
        {
            animationState = AnimationState.Idle;
        }

        // running
        if (movement.running)
        {
            animationState = AnimationState.Running;
        }

        // jumping
        if (movement.jumping)
        {
            animationState = AnimationState.Jumping;
        }

        // spellcasting TODO: add a way to check if a spell is being cast
        if(Input.GetKey("e")|| Input.GetKey("c")| Input.GetKey("r")|| Input.GetKey("x")|| Input.GetKey("q"))
        {
            animationState = AnimationState.SpellCasting;
        }

        // falling
        if (movement.falling && !movement.touchingWall)
        {
            animationState = AnimationState.Falling;
        }

        // holding on a wall
        if (movement.touchingWall && !movement.grounded)
        {
            animationState = AnimationState.Grappling;
        }

    }
}
