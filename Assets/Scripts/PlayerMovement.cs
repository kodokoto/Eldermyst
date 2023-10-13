using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movement;
    private Rigidbody2D rb;
    public float speed;
    // Start is called before the first frame update

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    // void OnMove(InputValue value) {
    //     moveValue = value.Get<Vector2>();
    // }

    void Run() {
        
    }

    void OnMove(InputValue value) {
        movement = value.Get<Vector2>();
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
