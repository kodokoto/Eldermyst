using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    // tag to make immune to the projectile
    public string targetTag;
    //
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

    // Destroy the projectile if it hits an enemy
    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.CompareTag(targetTag) )
        {
            other.gameObject.GetComponent<ITakeDamage>().TakeDamage(10);
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("Solid"))
        {
            Destroy(gameObject);
        }
    }
}
