using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float spitForce = 10f;

    void Start()
    {
        // Spit the projectile forward when spawned
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * spitForce;
    }
}
