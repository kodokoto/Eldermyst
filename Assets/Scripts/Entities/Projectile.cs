using NUnit.Framework.Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;

    // tag to make immune to the projectile
    [SerializeField] private LayerMask targetLayerMask;
    //
    void Start()
    {   
        rb.velocity = speed * transform.forward;
    }

    void Update()
    {
        // Destroy the projectile if it goes off screen based on camera view
        if (Camera.main.WorldToViewportPoint(transform.position).x > 1)
        {
            Destroy(gameObject);
        }
    }

    // Destroy the projectile if it hits an enemy
    void OnTriggerEnter(Collider other)
    {        
        // if other has the same layermask as targetLayer, take damage
        if (targetLayerMask == (targetLayerMask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<ITakeDamage>().TakeDamage(50);
            Destroy(gameObject);
        } 
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("StickyWall"))
        {
            Destroy(gameObject);
        }
    }
}
