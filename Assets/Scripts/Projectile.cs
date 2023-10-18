using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void Update()
    {
        // Destroy the projectile if it goes off screen
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
