using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    // Start is called before the first frame update

    void onMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector2 movement = new Vector2(moveValue.x, moveValue.y);

        GetComponent<Rigidbody2D>().AddForce(movement * speed * Time.fixedDeltaTime);
    }
}
